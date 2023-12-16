using System.Net.WebSockets;
using System.Text;
using ConsoleClient.Models;

string url = "ws://localhost:5275/Chat/Connect";
ClientWebSocket clientWebSocket = new ClientWebSocket();

try
{
    await clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
    Console.WriteLine("Connected to server.");
    StartReceive();
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
    return;
}

while (true)
{
    Console.WriteLine("Message:");
    var text = Console.ReadLine();
    await SendMessage(text);
}

async Task SendMessage(string message)
{
    if (clientWebSocket.State != WebSocketState.Open)
    {
        Console.WriteLine("WebSocket connection is not open.");
        return;
    }

    try
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine("Message sent successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to send message: {ex.Message}");
    }
}

async Task StartReceive()
{
    byte[] buffer = new byte[1024];

    try
    {
        while (clientWebSocket.State == WebSocketState.Open)
        {
            var result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received message: {message}");
                // Здесь можно обработать полученное сообщение
            }
        }
    }
    catch (WebSocketException ex)
    {
        Console.WriteLine($"WebSocket connection closed: {ex.Message}");
    }
}
