using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebServer.Data;
using WebServer.Hubs;
using WebServer.Models;

namespace WebServer.Controllers;

public class ChatController : Controller
{
    private ServerDbContext dbContext;

    public ChatController(ServerDbContext serverDbContext)
    {
        dbContext = serverDbContext;
    }

    public async Task<IEnumerable<Chat>> GetAllChats([FromBody] User user)
    {
        var chats = dbContext.UsersChats
            .Where(u => u.UserId == user.Id)
            .Select(c => c.Chat)
            .ToList();

        return chats;
    }

    public async Task<IEnumerable<Chat>> GetAllChats(int UserId)
    {
        var chats = dbContext.UsersChats
            .Where(u => u.UserId == UserId)
            .Select(c => c.Chat)
            .ToList();

        return chats;
    }

    public async Task CreateChat([FromBody] UsersChats usersChats)
    {
        var ch = dbContext.Chats.FirstOrDefault(c => c.Name == usersChats.Chat.Name);
        if (ch != null)
            return;

        dbContext.Chats.Add(usersChats.Chat);
        dbContext.UsersChats.Add(usersChats);

        dbContext.SaveChanges();
    }

    public async Task<IActionResult> Chat(Chat chat)
    {
        ViewBag.Chats = await GetAllChats(1);
        ViewBag.Chat = chat;
        return View();
    }

    public async Task<IActionResult> Chats()
    {
        ViewBag.Chats = await GetAllChats(1);
        return View();
    }

    public async Task<IActionResult> Main()
    {
        ViewBag.Chats = await GetAllChats(1);
        return View();
    }
}
