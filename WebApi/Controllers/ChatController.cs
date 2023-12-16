using Server.Models;
using Server.Main;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Diagnostics;

namespace WebApi.Controllers;

public class ChatController : Controller
{
    WebSocket webSocket;

    public async Task Connect()
    {
        Debug.WriteLine("try connect");
        webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        CancellationToken ct = HttpContext.RequestAborted;
        await Acceptor(webSocket, ct);
    }

    public async Task Acceptor(WebSocket webSocket, CancellationToken ct)
    {
        byte[] buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open && !ct.IsCancellationRequested)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var Text = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Message message = new Message();
                SendMessage(Text);
            }
        }

        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", ct);
    }

    public async Task SendMessage(String message)
    {
        //Linker.SendMessage(message);

        if (webSocket.State == WebSocketState.Open)
        {
            byte[] buff = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buff, 0, buff.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
