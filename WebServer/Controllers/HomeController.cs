using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ServerDbContext dbContext;

    public HomeController(ILogger<HomeController> logger, ServerDbContext serverDbContext)
    {
        _logger = logger;
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

    public async Task CreateChat([FromBody] UsersChats usersChats)
    {
        var ch = dbContext.Chats.FirstOrDefault(c => c.Name == usersChats.Chat.Name);
        if (ch != null)
            return;

        dbContext.Chats.Add(usersChats.Chat);
        dbContext.UsersChats.Add(usersChats);

        dbContext.SaveChanges();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
