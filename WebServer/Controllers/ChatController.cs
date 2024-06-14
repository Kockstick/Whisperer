using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebServer.Data;
using WebServer.Enums;
using WebServer.Hubs;
using WebServer.Models;

namespace WebServer.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IWebHostEnvironment hostingEnvironment;
    private IConfiguration configuration;
    private ServerDbContext dbContext;

    public ChatController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration, ServerDbContext serverDbContext)
    {
        this.hostingEnvironment = hostingEnvironment;
        this.configuration = configuration;
        dbContext = serverDbContext;
    }

    [HttpPost]
    public async Task<IActionResult> UploadMessageImage()
    {
        try
        {
            var file = Request.Form.Files[0];

            var projectFolderPath = hostingEnvironment.ContentRootPath;
            var uploadFolderPath = configuration["UploadSettings:UploadMessageImagePath"];
            string uniqueFileName = GenerateUniqueFileName(file.FileName, file.OpenReadStream());
            string fullPath = projectFolderPath + uploadFolderPath;

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            var filePath = Path.Combine(fullPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/{uploadFolderPath}/{uniqueFileName}";
            imageUrl = imageUrl.Replace("\\wwwroot", "");
            return Ok(new { imageUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadChatImage()
    {
        try
        {
            var file = Request.Form.Files[0];

            var projectFolderPath = hostingEnvironment.ContentRootPath;
            var uploadFolderPath = configuration["UploadSettings:UploadChatImagePath"];
            string uniqueFileName = GenerateUniqueFileName(file.FileName, file.OpenReadStream());
            string fullPath = projectFolderPath + uploadFolderPath;

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            var filePath = Path.Combine(fullPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var imageUrl = $"{Request.Scheme}://{Request.Host}/{uploadFolderPath}/{uniqueFileName}";
            imageUrl = imageUrl.Replace("\\wwwroot", "");
            return Ok(new { imageUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex}");
        }
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
        chat.Description = createModel.Description;
        chat.Image = createModel.Image;
        dbContext.Chats.Add(chat);

        dbContext.SaveChanges();

        var usersChats = new UsersChats();
        usersChats.ChatId = chat.Id;
        usersChats.UserId = getUserId();
        usersChats.Root = Root.Owner;
        dbContext.UsersChats.Add(usersChats);

        foreach (var item in createModel.UsersId)
        {
            usersChats = new UsersChats();
            usersChats.ChatId = chat.Id;
            usersChats.UserId = item;
            usersChats.Root = Root.Admin;
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

        var userChat = dbContext.UsersChats.FirstOrDefault(u => u.UserId == user.Id && u.ChatId == chat.Id);
        if (userChat == null)
            return RedirectToAction("Main");

        ViewBag.Chat = chat;
        ViewBag.User = user;
        return View();
    }

    public async Task<IActionResult> Main()
    {
        var user = getUser();
        if (user == null)
            return RedirectToAction("Logout", "Account");
        ViewBag.User = user;
        return View();
    }

    [HttpGet]
    public IActionResult Expand(Chat chat)
    {
        var _chat = dbContext.Chats.Find(chat.Id);
        if (_chat == null)
            return RedirectToAction("Main");
        ViewBag.Chat = _chat;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromBody] CreateChatModel createModel)
    {
        Chat chat = dbContext.Chats.Find(createModel.Id);
        if (chat == null)
            return BadRequest("Chat not found");

        chat.Name = createModel.Name;
        chat.Description = createModel.Description;
        chat.Image = createModel.Image;
        dbContext.Chats.Update(chat);

        dbContext.SaveChanges();

        foreach (var item in dbContext.UsersChats.Where(c => c.ChatId == createModel.Id).ToList())
        {
            if (createModel.UsersId.Contains(item.UserId))
                continue;
            dbContext.UsersChats.Remove(item);
        }

        dbContext.SaveChanges();

        return RedirectToAction("Chat", chat);
    }

    [HttpGet]
    public async Task<IActionResult> Invite(int chatId)
    {
        var existUserChat = dbContext.UsersChats.FirstOrDefault(u => u.UserId == getUserId() && u.ChatId == chatId);
        if (existUserChat == null)
        {
            var usersChats = new UsersChats();
            usersChats.ChatId = chatId;
            usersChats.UserId = getUserId();
            usersChats.Root = Root.Admin;
            dbContext.UsersChats.Add(usersChats);

            dbContext.SaveChanges();
        }

        var chat = dbContext.Chats.Find(chatId);

        return RedirectToAction("Chat", chat);
    }

    [HttpPost]
    public async Task<IActionResult> CreaterHelpChat()
    {
        try
        {
            var manager = dbContext.Users.FirstOrDefault(u => u.Login == "manager@mail.ru");
            var user = getUser();

            var chat = new Chat()
            {
                Name = manager.Name + " - " + user.Name,
                Description = "",
                Image = ""
            };
            dbContext.Chats.Add(chat);
            dbContext.SaveChanges();

            var usersChatManager = new UsersChats()
            {
                ChatId = chat.Id,
                UserId = manager.Id
            };

            var usersChatClient = new UsersChats()
            {
                ChatId = chat.Id,
                UserId = user.Id
            };

            dbContext.UsersChats.Add(usersChatManager);
            dbContext.UsersChats.Add(usersChatClient);
            dbContext.SaveChanges();

            return RedirectToAction("Chat", chat);
        }
        catch { }

        return RedirectToAction("Main");
    }

    [HttpPost]
    public async Task<List<User>> GetUsers([FromBody] int chatId)
    {
        List<User> users = new List<User>();
        users = dbContext.UsersChats.Where(c => c.ChatId == chatId)?.Select(u => u.User).ToList();
        return users;
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

    private string GenerateUniqueFileName(string originalFileName, Stream fileStream)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(fileStream);

            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            string fileExtension = Path.GetExtension(originalFileName);
            string uniqueFileName = hashString + fileExtension;

            return uniqueFileName;
        }
    }
}
