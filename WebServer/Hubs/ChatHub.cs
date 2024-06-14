using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Hubs;

public class ChatHub : Hub<IChatHub>
{
    ServerDbContext dbContext;

    public ChatHub(ServerDbContext serverDbContext)
    {
        dbContext = serverDbContext;
    }

    public async Task Send(Message message)
    {
        try
        {
            var chat = dbContext.Chats.Find(message.ChatId);
            message.Sender = dbContext.Users.Find(message.SenderId);

            if (message.ReplyMessageId != null)
                message.ReplyMessage = dbContext.Messages.Find(message.ReplyMessageId);

            dbContext.Messages.Add(message);
            dbContext.SaveChanges();

            await Clients.Group(chat.Id.ToString()).ReceiveMessage(message); // OthersInGroup(chat.Name).SendAsync("SendMessage", message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public async Task Edit(Message message)
    {
        try
        {
            var chat = dbContext.Chats.Find(message.ChatId);

            var existMessage = dbContext.Messages.Find(message.Id);
            existMessage.Text = message.Text;
            existMessage.File = message.File;
            dbContext.SaveChanges();

            await Clients.Group(chat.Id.ToString()).EditMessage(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public async Task Delete(Message message)
    {
        try
        {
            var chat = dbContext.Chats.Find(message.ChatId);

            var existMessage = dbContext.Messages.Find(message.Id);
            if (existMessage == null)
                return;

            existMessage.IsDeleted = true;
            dbContext.SaveChanges();

            await Clients.Group(chat.Id.ToString()).DeleteMessage(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception);
    }
}
