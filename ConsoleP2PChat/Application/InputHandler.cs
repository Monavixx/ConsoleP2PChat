using System.Net;
using System.Text;
using ConsoleP2PChat.Domain;

namespace ConsoleP2PChat.Application;

public class InputHandler (IChatRepository chatRepository, ChatContext chatContext)
{
    public async Task HandleAsync(string input)
    {
        if (input.StartsWith("/"))
        {
            if (input == "/quit")
            {
                Quited?.Invoke();
            }

            return;
        }

        await chatContext.CurrentSession!.SendAsync(Encoding.UTF8.GetBytes(input));
        chatRepository.AddMessage(chatContext.CurrentRemoteAddress!, new Message(input, DateTime.Now, MessageDirection.Outcoming));
    }

    public event Action? Quited;
}