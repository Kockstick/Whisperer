namespace WebServer.Models;

public class CreateChatModel
{
    public string Name { get; set; }
    public List<int> UsersId { get; set; } = new List<int>();
}
