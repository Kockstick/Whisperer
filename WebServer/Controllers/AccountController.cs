using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel loginModel)
    {
        return RedirectToAction("Main", "Chat");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
}
