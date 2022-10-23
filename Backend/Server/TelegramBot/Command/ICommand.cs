namespace TelegramBot.Command;

public interface ICommand
{
    abstract public Task Execute();
}