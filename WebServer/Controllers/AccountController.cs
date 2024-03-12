using Microsoft.AspNetCore.Mvc;
using WebServer.Data;
using WebServer.Models;

namespace WebServer.Controllers;

public class AccountController : Controller
{
    private ServerDbContext dbContext;

    public AccountController(ServerDbContext serverDbContext)
    {
        dbContext = serverDbContext;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Login == loginModel.Login);

        if (user == null)
            return RedirectToAction("Login");
        if (user.Password != loginModel.Password)
            return RedirectToAction("Login");

        HttpContext.Response.Cookies.Append("id", user.Id.ToString());
        return RedirectToAction("Main", "Chat", user);
    }

    [HttpGet]
    public IActionResult SingUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SingUp(User user)
    {
        try
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
            return RedirectToAction("Login");
        }
        catch
        {
            return RedirectToAction("SingUp");
        }
    }
}
