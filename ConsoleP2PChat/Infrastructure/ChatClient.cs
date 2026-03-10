using System.Net.Sockets;
using System.Text;
using Timer = System.Timers.Timer;

namespace ConsoleP2PChat.Infrastructure;

public class ChatClient
{
    private TcpClient _client = null!;
    public async Task ConnectAndRunAsync(string remoteAddress, int port)
    {
        if (_client is not null) _client.Close();
        
        _client = new TcpClient();
        await _client.ConnectAsync(remoteAddress, port);
        Timer timer = new Timer(3000);
        timer.AutoReset = true;
        timer.Elapsed += async (sender, args) =>
        {
            var stream = _client.GetStream();
            byte[] msg = Encoding.UTF8.GetBytes("Hello World!");
            await stream.WriteAsync(BitConverter.GetBytes(msg.Length));
            await stream.WriteAsync(msg);
            stream.FlushAsync();
        };
        timer.Start();
        var stream = _client.GetStream();
        while (true)
        {
            byte[] msgLenBytes = new byte[4];
            await stream.ReadExactlyAsync(msgLenBytes);
            byte[] msg = new byte[BitConverter.ToInt32(msgLenBytes)];
            await stream.ReadExactlyAsync(msg);
            string message = Encoding.UTF8.GetString(msg);
            Console.WriteLine(message);
            if (message == "exit") return;
        }
    }
}