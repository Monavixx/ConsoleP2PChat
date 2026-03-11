using System.Net.Sockets;
using ConsoleP2PChat.Application;

namespace ConsoleP2PChat.Infrastructure;

public class ChatSession : IChatSession, IDisposable
{
    private readonly TcpClient _tcpClient;
    private readonly IMessageHandler _messageHandler;

    public ChatSession(TcpClient tcpClient, IMessageHandler messageHandler)
    {
        _tcpClient = tcpClient;
        _messageHandler = messageHandler;
    }

    public async Task RunAsync(CancellationToken token)
    {
        await _messageHandler.OnClientConnected(this);
        await using var stream = _tcpClient.GetStream();
        var messageSizeBytes = new byte[4];
        try
        {
            while (!token.IsCancellationRequested)
            {
                await stream.ReadExactlyAsync(messageSizeBytes, token);
                int messageSize = BitConverter.ToInt32(messageSizeBytes);
                byte[] message = new byte[messageSize];
                await stream.ReadExactlyAsync(message, token);
                await _messageHandler.OnMessageReceived(message, this);
            }
        }
        catch (OperationCanceledException)
        { }
        catch (EndOfStreamException)
        { }
        finally
        {
            await _messageHandler.OnClientDisconnected(this);
        }
    }

    public string RemoteAddress => _tcpClient.Client.RemoteEndPoint.ToString();
    public async Task SendAsync(byte[] message)
    {
        var stream = _tcpClient.GetStream();
        await stream.WriteAsync(BitConverter.GetBytes(message.Length));
        await stream.WriteAsync(message);
    }

    public void Dispose()
    {
        _tcpClient.Dispose();
    }
}