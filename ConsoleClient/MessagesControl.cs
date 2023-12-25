using System.Net.Http.Json;
using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient;

public class MessagesControl
{
    string url = "http://localhost:5205/chat/";
    string homeUrl = "http://localhost:5205/chat/";

    private HubConnection connection;
    private User user;
    private CommandsControl cmd;

    public MessagesControl()
    {
        connection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
        connection.StartAsync();
    }

    public async Task<Message> ReceiveMessages(User user)
    {
        this.user = user;
        cmd = new CommandsControl(user);
        await cmd.Connect(connection, user.CurrentChat.Name);
        Console.WriteLine("Вы вошли в чат");

        connection.On<Message>("SendMessage", (message) =>
        {
            Console.WriteLine(message.Text);
        });

        while (user.CurrentChat != null)
        {
            var text = Console.ReadLine();

            if (cmd.Command(text))
                continue;

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
