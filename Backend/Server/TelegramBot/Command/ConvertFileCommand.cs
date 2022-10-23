using System.Text;
using Domain.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Document = Domain.Model.Document;
using File = System.IO.File;
using User = Domain.Model.User;

namespace TelegramBot.Command;

public class ConvertFileCommand : ICommand
{
    private readonly Message _message;
    private readonly IUserServices _userServices;
    private readonly IConverterService _converterService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IDocumentService _documentService;

    private const string MESSAGE = "This file type is not supported.";
    private const string FILE_PATH = "File/file.html";
    private const string NEW_FILE_NAME = "ConvertedFile.pdf";

    public ConvertFileCommand(Message message, IUserServices userServices, IConverterService converterService, ITelegramBotClient telegramBotClient, IDocumentService documentService)
    {
        _message = message;
        _userServices = userServices;
        _converterService = converterService;
        _telegramBotClient = telegramBotClient;
        _documentService = documentService;
    }

    public async Task Execute()
    {
        var chatId = _message.Chat.Id;
        var fileId = _message.Document.FileId;
        var mineType = _message.Document.MimeType;
        
        if (mineType != "text/html")
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, MESSAGE);
            return;
        }
        
        using (var fileStream = File.OpenWrite(FILE_PATH))
        {
            var fileInfo = await _telegramBotClient.GetInfoAndDownloadFileAsync(
                fileId: fileId,
                destination: fileStream
            );
        }

        var readFileStream = await File.ReadAllBytesAsync(FILE_PATH);
        string textFromFile = Encoding.Default.GetString(readFileStream);
        
        var convertedFileStream = _converterService.ToPdf(textFromFile);
        var inputOnlineFile = new InputOnlineFile(convertedFileStream,NEW_FILE_NAME);
        
       await _telegramBotClient.SendDocumentAsync(chatId, inputOnlineFile);

       var user = await _userServices.Get(chatId);
       if (ReferenceEquals(user,null))
       {
            user = new User(chatId);
           var newUser = await _userServices.Add(user);
       }
       
       var readyDocument = ReadAllBytes(convertedFileStream);
       var newDocument = new Document(chatId, readFileStream, readyDocument);
       await _documentService.Add(newDocument);
    }
    
    private byte[] ReadAllBytes(Stream instream)
    {
        if (instream is MemoryStream)
            return ((MemoryStream) instream).ToArray();

        using (var memoryStream = new MemoryStream())
        {
            instream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}