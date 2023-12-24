using ConsoleClient.Models;

namespace Server.Models;

public class IpAddress
{
    public int Id { get; set; }
    public string Ip { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
}
