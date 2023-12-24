using WebServer.Models;

namespace WebServer.Models;

public class UsersChats
{
    public int Id { get; set; }
    public int ChatId { get; set; }
    public int UserId { get; set; }
    public int RootId { get; set; }

    public Chat Chat { get; set; }
    public User User { get; set; }
    public Root Root { get; set; }
}
