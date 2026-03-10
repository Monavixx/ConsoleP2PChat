using ConsoleP2PChat.Application;

namespace ConsoleP2PChat.Presentation;

public class MessageInput (InputHandler inputHandler)
{
    public Task RunAsync(CancellationToken token)
    {
        return Task.Run(() =>
        {
            while (!token.IsCancellationRequested)
            {
                string input = Console.ReadLine() ?? "";
                _ = inputHandler.HandleAsync(input);
            }
        }, token);
    }
}