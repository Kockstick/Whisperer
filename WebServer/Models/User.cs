
using WebServer.Models;

namespace WebServer.Models;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public int? CurrentChatId { get; set; }

    public Chat? CurrentChat { get; set; }
    public List<UsersChats> UsersChats { get; set; }
}
