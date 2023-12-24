using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleClient.Models;

namespace Server.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CreatorId { get; set; }

    public User Creator { get; set; }
}