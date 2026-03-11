using ConsoleP2PChat.Application;

namespace ConsoleP2PChat.Presentation;

public class MessageInput (IInputHandler inputHandler)
{
    public Task RunAsync(CancellationToken? token = null)
    {
        token ??= new CancellationToken(false);
        return Task.Run(() =>
        {
            while (!token.Value.IsCancellationRequested)
            {
                string input = Console.ReadLine() ?? "";
                _ = inputHandler.HandleAsync(input);
            }
        }, token.Value);
    }
}