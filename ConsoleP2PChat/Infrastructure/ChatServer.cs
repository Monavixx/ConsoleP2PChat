using System.Net;
using System.Net.Sockets;
using ConsoleP2PChat.Application;
using ConsoleP2PChat.Presentation;

namespace ConsoleP2PChat.Infrastructure;

public class ChatServer : IDisposable
{
    private readonly IMessageHandler _messageHandler;
    private readonly ChatContext _chatContext;
    private TcpListener _listener;
    private CancellationTokenSource _tokenSource;
    private CancellationToken _token;

    public ChatServer(int port, IMessageHandler messageHandler, ChatContext chatContext)
    {
        _messageHandler = messageHandler;
        _chatContext = chatContext;
        _listener = new TcpListener(IPAddress.Any, port);
        _tokenSource = new CancellationTokenSource();
        _token = _tokenSource.Token;
    }

    public async Task StartAsync()
    {
        _listener.Start();
        _token.Register(() => _listener.Stop());
        while (!_token.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync(_token);
            var session = new ChatSession(client, _messageHandler);
            _chatContext.CurrentSession ??= session;

            _ = session.RunAsync(_token).ContinueWith(_ => session.Dispose(), _token);
        }
    }

    public void Dispose()
    {
        _listener.Dispose();
        _tokenSource.Cancel();
        _tokenSource.Dispose();
    }
}