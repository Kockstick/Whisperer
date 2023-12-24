using System.Net.Http.Json;
using System.Text;
using ConsoleClient;
using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

User user = new User()
{
    Id = 1,
    Name = "Kockstik",
    Login = "admin",
    Password = "admin"
};

var chatsControl = new ChatsControl();
var messagesControl = new MessagesControl();

while (true)
{
    if (user.CurrentChat == null)
    {
        user.CurrentChat = await chatsControl.GetChatAsync();
        continue;
    }

    while (user.CurrentChat != null)
    {
        await messagesControl.ReceiveMessages(user);
    }
}