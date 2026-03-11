using System.Net.Sockets;
using System.Text;
using ConsoleP2PChat.Application;
using ConsoleP2PChat.Presentation;
using Timer = System.Timers.Timer;

namespace ConsoleP2PChat.Infrastructure;

public class ChatClient (ChatContext chatContext, IMessageHandler messageHandler)
{
    private TcpClient _client = null!;
    private CancellationTokenSource _tokenSource;
    private CancellationToken _token;
    public async Task ConnectAndRunAsync(string remoteAddress, int port)
    {
        _tokenSource = new CancellationTokenSource();
        _token = _tokenSource.Token;
        if (_client is not null) _client.Close();
        
        _client = new TcpClient();
        await _client.ConnectAsync(remoteAddress, port, _token);
        ChatSession session = new ChatSession(_client, messageHandler);
        chatContext.CurrentSession = session;
        await session.RunAsync(_token);
        session.Dispose();
        // var stream = _client.GetStream();
        // while (true)
        // {
        //     byte[] msgLenBytes = new byte[4];
        //     await stream.ReadExactlyAsync(msgLenBytes, _token);
        //     byte[] msg = new byte[BitConverter.ToInt32(msgLenBytes)];
        //     await stream.ReadExactlyAsync(msg, _token);
        //     await messageHandler.OnMessageReceived(msg, chatContext.CurrentSession!);
        // }
    }
}