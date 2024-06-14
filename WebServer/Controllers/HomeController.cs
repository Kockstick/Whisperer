using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ServerDbContext dbContext;

    public HomeController(ILogger<HomeController> logger, ServerDbContext serverDbContext)
    {
        _logger = logger;
        dbContext = serverDbContext;
    }

    public IActionResult Index()
    {
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public List<User> Search([FromBody] string searchText)
    {
        List<User> users = new List<User>();
        try
        {
            if (searchText.Length > 0)
            {
                if (searchText != null)
                    users = dbContext.Users.Where(u => u.Id != getUserId() && u.Name.ToLower().Contains(searchText.ToLower())).ToList();
            }
            else
            {
                users = dbContext.Contacts.Where(u => u.UserId == getUserId()).Select(u => u.ContactUser).ToList();
            }

        }
        catch { }
        return users;
    }

    [HttpPost]
    public bool AddContact([FromBody] User user)
    {
        try
        {
            Contact contact = new Contact();
            contact.UserId = getUserId();
            contact.ContactUserId = user.Id;
            dbContext.Contacts.Add(contact);
            dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpPost]
    public bool RemoveContact([FromBody] User user)
    {
        try
        {
            Contact contact = dbContext.Contacts.FirstOrDefault(u => u.ContactUserId == user.Id);
            if (contact == null)
                return false;

            dbContext.Contacts.Remove(contact);
            dbContext.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    [HttpGet]
    public List<User> GetContacts()
    {
        return dbContext.Contacts.Where(u => u.UserId == getUserId()).Select(u => u.ContactUser).ToList();
    }

    [HttpGet]
    public IActionResult Settings()
    {
        try
        {
            var user = getUser();
            ViewBag.User = user;
        }
        catch { }
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
