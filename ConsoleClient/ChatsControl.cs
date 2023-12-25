using System.Net.Http.Json;
using ConsoleClient.Models;

namespace ConsoleClient;

public class ChatsControl
{
    string url = "http://localhost:5205/home/";
    private User user;
    private CommandsControl cmd;

    private List<Chat> chats = new List<Chat>();

    public async Task<Chat> GetChatAsync(User user)
    {
        this.user = user;
        cmd = new CommandsControl(user);
        cmd.OnUpdateChats += OnUpdateChats;
        cmd.UpdateChats();
        Chat chat = null;

        while (chat == null)
        {
            Console.WriteLine("Выбери чат или введи команду");
            var text = Console.ReadLine();

            if (cmd.Command(text))
                continue;

            chat = chats.FirstOrDefault(c => c.Name == text);
        }

        return chat;
    }

    private void OnUpdateChats(List<Chat> chats)
    {
        this.chats = chats;
    }

}