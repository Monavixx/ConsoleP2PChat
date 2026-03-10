using ConsoleP2PChat.Domain;

namespace ConsoleP2PChat.Application;

public interface IChatRepository
{
    void AddMessage(string remoteAddress, Message message);
    event Action<string, Message> MessageAdded;
    IReadOnlyList<Message> GetMessages(string remoteAddress);
}