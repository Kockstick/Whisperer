using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Controllers;

[Authorize]
public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private ServerDbContext dbContext;

    public UserController(ILogger<HomeController> logger, ServerDbContext serverDbContext)
    {
        _logger = logger;
        dbContext = serverDbContext;
    }

    [HttpGet]
    public IActionResult Profile()
    {
        var user = getUser();
        ViewBag.User = user;
        return View();
    }

    [HttpPost]
    public IActionResult ChangeName([FromBody] string name)
    {
        try
        {
            var user = getUser();
            user.Name = name;
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost]
    public IActionResult IsManager()
    {
        try
        {
            var user = getUser();
            if (user.Login == "manager@mail.ru")
                return Ok("Is Manager");
            return BadRequest("No Manager");
        }
        catch
        {
            return BadRequest("No Manager");
        }
    }

    private User getUser()
    {
        int id = getUserId();
        if (id == 0)
            return null;
        var user = dbContext.Users
                            .Include(u => u.UsersChats)
                                .ThenInclude(uc => uc.Chat) // Включаем связанные данные чатов
                            .Include(u => u.Contacts)
                                .ThenInclude(uc => uc.ContactUser)
                            .FirstOrDefault(u => u.Id == id);
        return user;
    }

    private int getUserId()
    {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id")?.Value);
        return id;
    }
}
