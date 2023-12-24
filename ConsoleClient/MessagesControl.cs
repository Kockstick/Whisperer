using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient;

public class MessagesControl
{
    string url = "http://localhost:5205/chat/";

    HubConnection connection;

    public MessagesControl()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
        connection.StartAsync();
    }

    public async Task<Message> ReceiveMessages(User user)
    {
        await connection.InvokeAsync("JoinAsync", user.CurrentChat.Name);
        Console.WriteLine("Вы вошли в чат");

        connection.On<Message>("SendMessage", (message) =>
        {
            Console.WriteLine(message.Text);
        });

        while (user.CurrentChat.Name != null)
        {
            var text = Console.ReadLine();

            Message message = new Message()
            {
                ChatId = 1,
                SenderId = 1,
                Date = DateTime.Now,
                Text = text,
                Sender = user,
                Chat = user.CurrentChat
            };

            await connection.InvokeAsync("Send", message);
        }

        return null;
    }
}
