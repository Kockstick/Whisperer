using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Database;
using Server.Models;

namespace Server.Main;

public class Linker
{
    public static void SendMessage(Message message)
    {
        using (var context = new WContext())
        {
            var chat = context.Chats.Find(message.ChatId);
            if (chat == null)
                return;
            chat.SendMessage(message);
        }
    }
}
