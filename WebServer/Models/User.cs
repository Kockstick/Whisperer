
using System.Text.Json.Serialization;
using WebServer.Models;

namespace WebServer.Models;

public class User
{
    public int Id { get; set; }
    [JsonIgnore]
    public string Login { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public string Name { get; set; }

    public List<UsersChats> UsersChats { get; set; }
    public List<Contact> Contacts { get; set; }
}
