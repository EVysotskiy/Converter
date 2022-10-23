using Domain.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Command;

public class CommandFactory : ICommandFactory
{
    private readonly IUserServices _userServices;
    private readonly IConverterService _converterService;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IDocumentService _documentService;

    public CommandFactory(IUserServices userServices, IConverterService converterService, ITelegramBotClient telegramBotClient, IDocumentService documentService)
    {
        _userServices = userServices;
        _converterService = converterService;
        _telegramBotClient = telegramBotClient;
        _documentService = documentService;
    }
    public ICommand Create(Message message)
    {
        var commandType = GetTypeByMessage(message);

        return commandType switch
        {
            CommandType.Convert => new ConvertFileCommand(message,_userServices,_converterService,_telegramBotClient,_documentService),
            CommandType.GetFiles => new GetFilesCommand(message,_userServices,_converterService,_documentService,_telegramBotClient),
            CommandType.None => new UnregisteredCommand(message,_telegramBotClient),
            _ => throw new ArgumentException("This command type has no handler")
        };
    }

    private CommandType GetTypeByMessage(Message message)
    {
        var isText = message.Text != null;
        var isDocument = message.Document != null;

        if (isDocument)
        {
            return CommandType.Convert;
        }

        if (isText)
        {
            return message.Text switch
            {
                "/getfiles" => CommandType.GetFiles,
                _ => CommandType.None
            };
        }

        return CommandType.None;
    }
}