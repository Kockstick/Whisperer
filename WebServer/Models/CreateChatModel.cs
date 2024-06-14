namespace WebServer.Models;

public class CreateChatModel
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public List<int> UsersId { get; set; } = new List<int>();
}
