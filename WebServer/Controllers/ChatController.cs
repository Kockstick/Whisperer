using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebServer.Data;
using WebServer.Hubs;
using WebServer.Models;

namespace WebServer.Controllers;

[Authorize]
public class ChatController : Controller
{
    private ServerDbContext dbContext;

    public ChatController(ServerDbContext serverDbContext)
    {
        dbContext = serverDbContext;
    }

    [HttpGet]
    public async Task<List<Chat>> GetAllChats()
    {
        var chats = dbContext.UsersChats
            .Where(u => u.UserId == getUser().Id)
            .Select(c => c.Chat)
            .ToList();

        return chats;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateChatModel createModel)
    {
        Chat chat = new Chat();
        chat.Name = createModel.Name;
        dbContext.Chats.Add(chat);

        dbContext.SaveChanges();

        var usersChats = new UsersChats();
        usersChats.ChatId = chat.Id;
        usersChats.UserId = getUserId();
        usersChats.RootId = 1;
        dbContext.UsersChats.Add(usersChats);

        foreach (var item in createModel.UsersId)
        {
            usersChats = new UsersChats();
            usersChats.ChatId = chat.Id;
            usersChats.UserId = item;
            usersChats.RootId = 1;
            dbContext.UsersChats.Add(usersChats);
        }

        dbContext.SaveChanges();

        return RedirectToAction("Chat", chat);
    }

    public async Task<IActionResult> Chat(Chat chat)
    {
        var user = getUser();
        if (user == null)
            return RedirectToAction("Login", "Account");
        ViewBag.Chat = chat;
        ViewBag.User = user;
        return View();
    }

    public async Task<IActionResult> Main()
    {
        var user = getUser();
        if (user == null)
            return RedirectToAction("Login", "Account");
        ViewBag.User = user;
        ViewBag.Chats = await GetAllChats();
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
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        return id;
    }
}
