using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers;

[Authorize]
public class UserController : Controller
{
    [HttpGet]
    public IActionResult Profile()
    {
        return View();
    }
}
