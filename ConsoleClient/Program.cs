using System.Net.Http.Json;
using System.Text;
using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Server.Models;

//string url = "ws://localhost:5275/api/Message/";
string url = "http://localhost:5275/chat/";

HubConnection connection = new HubConnectionBuilder()
    .WithUrl(url)
    .Build();

connection.On<string>("Receive", (message) =>
{
    Console.WriteLine(message);
});

await connection.StartAsync();
Console.WriteLine("Вы вошли в чат");

while (true)
{
    var text = Console.ReadLine();
    connection.InvokeAsync("Send", text);
}