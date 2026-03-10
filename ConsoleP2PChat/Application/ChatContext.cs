namespace ConsoleP2PChat.Application;

public class ChatContext
{
    public IChatSession? CurrentSession
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                SessionChanged?.Invoke();
            }
        }
    }
    public event Action? SessionChanged;
    public string? CurrentRemoteAddress => CurrentSession?.RemoteAddress;
}