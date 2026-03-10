using System.Text;
using ConsoleP2PChat.Domain;

namespace ConsoleP2PChat.Application;

public class MessageHandler(IChatRepository chatRepository) : IMessageHandler
{
    public async Task OnMessageReceived(byte[] message, IChatSession chatSession)
    {
        string strMessage = Encoding.UTF8.GetString(message);
        chatRepository.AddMessage(chatSession.RemoteAddress, new Message(strMessage, DateTime.Now, MessageDirection.Incoming));
    }

    public async Task OnClientDisconnected(IChatSession chatSession)
    {
    }

    public async Task OnClientConnected(IChatSession chatSession)
    {
    }
}