using System.Net.Http.Json;
using System.Text;
using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

string url = "http://localhost:5205/chat/";

HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .Build();

connection.On<Message>("SendMessage", (message) =>
{
    Console.WriteLine(message.Text);
});

await connection.StartAsync();
await connection.InvokeAsync("JoinAsync", "MainChat");
Console.WriteLine("Вы вошли в чат");

while (true)
{
    var text = Console.ReadLine();

    Message message = new Message()
    {
        ChatId = 1,
        SenderId = 1,
        Date = DateTime.Now,
        Text = text
    };

    await connection.InvokeAsync("Send", message);
}