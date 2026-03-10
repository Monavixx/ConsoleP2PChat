using System.Net;
using System.Net.Sockets;
using ConsoleP2PChat.Application;

namespace ConsoleP2PChat.Infrastructure;

public class ChatServer : IDisposable
{
    private readonly IMessageHandler _messageHandler;
    private TcpListener _listener;

    public ChatServer(int port, IMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
        _listener = new TcpListener(IPAddress.Any, port);
    }
    public async Task StartAsync(CancellationToken token)
    {
        _listener.Start();
        token.Register(() => _listener.Stop());
        while (!token.IsCancellationRequested)
        {
            var client = await _listener.AcceptTcpClientAsync(token);
            var session = new ChatSession(client, _messageHandler);
            _ = session.RunAsync(token).ContinueWith(_ => session.Dispose(), token);
        }
    }

    public void Dispose()
    {
        _listener.Dispose();
    }
}