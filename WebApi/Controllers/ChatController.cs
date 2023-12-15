using Server.Models;
using Server.Main;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ChatController : Controller
{
    public string SendMessage(Message message)
    {
        Chat chat = new Chat();
        chat.Id = 0;
        chat.Name = "chat";
        chat.CreatorId = 0;

        Linker.SendMessage(message, chat);

        return message.Text;
    }
}
