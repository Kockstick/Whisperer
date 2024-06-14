using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        if (HttpContext.User.Identity.IsAuthenticated)
            return RedirectToAction("Main", "Chat");
        else
            return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        var user = dbContext.Users.FirstOrDefault(u => u.Login == loginModel.Login);

        if (user == null)
            return BadRequest("User was not found"); //Results.Unauthorized();//return RedirectToAction("Login");
        if (user.Password != loginModel.Password)
            return BadRequest("Incorrect password");  //Results.Unauthorized();//return RedirectToAction("Login");

        var claims = new List<Claim> { new Claim("Id", user.Id.ToString()), new Claim(ClaimTypes.Name, loginModel.Login) };

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        Console.WriteLine("Enter user " + HttpContext.User.FindFirst(ClaimTypes.Name)?.Value);

        HttpContext.Response.Cookies.Append("id", user.Id.ToString());
        HttpContext.Response.Cookies.Append("username", loginModel.Login);

        return RedirectToAction("Main", "Chat"); //Results.Json(response);
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
            if (dbContext.Users.FirstOrDefault(u => u.Login == user.Login) != null)
                return RedirectToAction("SingUp");

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return RedirectToAction("Login");
        }
        catch
        {
            return RedirectToAction("SingUp");
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
