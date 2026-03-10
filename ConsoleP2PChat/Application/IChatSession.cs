namespace ConsoleP2PChat.Application;

public interface IChatSession
{
    string RemoteAddress { get; }
    Task SendAsync(byte[] message);
}