using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Hubs;

public class ChatHub : Hub
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
            await Clients.OthersInGroup(chat.Name).SendAsync("SendMessage", message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    public async Task<Chat> JoinAsync(string chatName)
    {
        var chat = dbContext.Chats.FirstOrDefault(c => c.Name == chatName);
        if (chat == null)
            return null;

        await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
        return chat;
    }

    public async Task LeaveAsync(string chatName)
    {
        var chat = dbContext.Chats.FirstOrDefault(c => c.Name == chatName);
        if (chat == null)
            return;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
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
