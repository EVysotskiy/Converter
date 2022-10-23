using Telegram.Bot.Types;

namespace TelegramBot.Command;

public interface ICommandFactory
{
    public ICommand Create(Message message);
}