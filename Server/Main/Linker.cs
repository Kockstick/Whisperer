using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Models;

namespace Server.Main;

public class Linker
{
    public static void SendMessage(Message message, Chat chat)
    {
        chat.SendMessage(message);
    }
}
