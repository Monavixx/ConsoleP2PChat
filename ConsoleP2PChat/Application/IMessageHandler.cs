namespace ConsoleP2PChat.Application;

public interface IMessageHandler
{
    Task OnMessageReceived(byte[] message, IChatSession chatSession);
    Task OnClientDisconnected(IChatSession chatSession);
    Task OnClientConnected(IChatSession chatSession);
}