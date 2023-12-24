using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IEnumerable<Chat>> GetAllChats()
    {
        return dbContext.Chats;
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
