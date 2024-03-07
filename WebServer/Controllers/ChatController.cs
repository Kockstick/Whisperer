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
        var user = getUser();
        if (user == null)
            return RedirectToAction("Login", "Account");
        ViewBag.Chats = await GetAllChats(user.Id);
        ViewBag.Chat = chat;
        ViewBag.User = user;
        return View();
    }

    public async Task<IActionResult> Chats()
    {
        var user = getUser();
        if (user == null)
            return RedirectToAction("Login", "Account");
        ViewBag.User = user;
        ViewBag.Chats = await GetAllChats(user.Id);
        return View();
    }

    public async Task<IActionResult> Main()
    {
        var user = getUser();
        if (user == null)
            return RedirectToAction("Login", "Account");
        ViewBag.User = user;
        ViewBag.Chats = await GetAllChats(user.Id);
        return View();
    }

    private User getUser()
    {
        int id = getUserId();
        if (id == 0)
            return null;
        var user = dbContext.Users.FirstOrDefault(u => u.Id == id);
        return user;
    }

    private int getUserId()
    {
        if (!HttpContext.Request.Cookies.ContainsKey("id"))
            return 0;
        int id = Convert.ToInt32(HttpContext.Request.Cookies["id"]);
        return id;
    }
}
