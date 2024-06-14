using WebServer.Models;

namespace WebServer.Hubs;

public interface IChatHub
{
    public Task ReceiveMessage(Message message);
    public Task EditMessage(Message message);
    public Task DeleteMessage(Message message);
}
