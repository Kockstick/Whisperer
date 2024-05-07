using WebServer.Enums;
using WebServer.Models;

namespace WebServer.Models;

public class UsersChats
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public Root Root { get; set; } = Root.Guest;

    public Chat Chat { get; set; }
    public User User { get; set; }
}
