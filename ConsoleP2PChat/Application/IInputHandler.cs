namespace ConsoleP2PChat.Application;

public interface IInputHandler
{
    Task HandleAsync(string input);
    event Action? Quited;
}