using Domain.Model;
using Domain.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = Domain.Model.User;

namespace TelegramBot.Command;

public class GetFilesCommand : ICommand
{
    private readonly Message _message;
    private readonly IUserServices _userServices;
    private readonly IConverterService _converterService;
    private readonly IDocumentService _documentService;
    private readonly ITelegramBotClient _telegramBotClient;
     

    private const string USER_NOT_FOUND = "This user has no converted documents.\nSend me the file and I will convert it.";

    public GetFilesCommand(Message message, IUserServices userServices, IConverterService converterService, IDocumentService documentService, ITelegramBotClient telegramBotClient)
    {
        _message = message;
        _userServices = userServices;
        _converterService = converterService;
        _documentService = documentService;
        _telegramBotClient = telegramBotClient;
    }

    public async Task Execute()
    {
        var chatId = _message.Chat.Id;

        var user = await _userServices.Get(chatId);
        if (ReferenceEquals(user,null))
        {
            await _telegramBotClient.SendTextMessageAsync(chatId, USER_NOT_FOUND);
            var newUser = new User(chatId);
            await _userServices.Add(newUser);
            return;
        }

        var userDocuments = await _documentService.GetAll(user.IdChat);
        
        for (var i = 0; i < userDocuments.Count; i++)
        {
            var fileName = string.Format($"File {i + 1}.pdf");
            Stream stream = new MemoryStream(userDocuments[i].DocumentReady);
            var inputOnlineFile = new InputOnlineFile(stream,fileName);
            await _telegramBotClient.SendDocumentAsync(chatId, inputOnlineFile);
        }
    }
}