using ConsoleP2PChat.Application;
using ConsoleP2PChat.Domain;
using ConsoleP2PChat.Infrastructure;

namespace ConsoleP2PChat.Presentation;

public class ConsoleChat
{
    private IMessageHandler _messageHandler;
    private ChatServer _chatServer;
    private IChatRepository _chatRepository;
    ChatContext _chatContext;
    InputHandler _inputHandler;
    MessageInput _messageInput;
    
    public ConsoleChat()
    {
        _chatRepository = new InMemoryChatRepository();
        _messageHandler = new MessageHandler(_chatRepository);
        _chatContext = new ChatContext();
        _inputHandler = new InputHandler(_chatRepository, _chatContext);
        _messageInput = new MessageInput(_inputHandler);
        _inputHandler.Quited += InputHandlerOnQuited;
        _chatRepository.MessageAdded += ChatRepositoryOnMessageAdded;
    }

    private void ChatRepositoryOnMessageAdded(string remoteAddress, Message message)
    {
        if (message.Direction == MessageDirection.Incoming)
        {
            Console.WriteLine($"[From {remoteAddress} {message.ReceivedAt}]: {message.Text}");
        }
        else if (message.Direction == MessageDirection.Outcoming)
        {
            Console.WriteLine($"[To {remoteAddress} {message.ReceivedAt}]: {message.Text}");
        }
    }

    public async Task Run()
    {
        Console.WriteLine("Choose an action:" + Environment.NewLine +
                          "1) Run a server" + Environment.NewLine +
                          "2) Connect");
        bool actionReceived = false;
        while (!actionReceived)
        {
            actionReceived = true;
            string actionInput = Console.ReadLine() ?? "";
            switch (actionInput)
            {
                case "1":
                    await RunServer();
                    break;
                case "2":
                    await RunClient();
                    break;
                default:
                    actionReceived = false;
                    Console.WriteLine("Try again!");
                    break;
            }
        }
    }

    public async Task RunServer()
    {
        _chatServer = new ChatServer(7654, _messageHandler, _messageInput, _chatContext);
        await _chatServer.StartAsync();
    }

    private void InputHandlerOnQuited() => _chatServer.Dispose();

    public async Task RunClient()
    {
        ChatClient chatClient = new();
        string address = Console.ReadLine() ?? "";
        int port =  int.Parse(Console.ReadLine() ?? "7654");
        await chatClient.ConnectAndRunAsync(address, port);
    }
    
}