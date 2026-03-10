using ConsoleP2PChat.Application;
using ConsoleP2PChat.Domain;

namespace ConsoleP2PChat.Infrastructure;

public class InMemoryChatRepository : IChatRepository
{
    private readonly Dictionary<string, List<Message>> _messages = new();
    public event Action<string, Message>? MessageAdded;
    
    public void AddMessage(string remoteAddress, Message message)
    {
        lock (_messages)
        {
            if (_messages.TryGetValue(remoteAddress, out List<Message>? messages))
            {
                messages.Add(message);
            }
            else
            {
                _messages[remoteAddress] = [message];
            }

            MessageAdded?.Invoke(remoteAddress, message);
        }
    }
    
    public IReadOnlyList<Message> GetMessages(string remoteAddress)
    {
        lock(_messages)
            return _messages[remoteAddress].AsReadOnly();
    }
}