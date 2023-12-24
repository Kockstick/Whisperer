using System.Net.Http.Json;
using ConsoleClient.Models;

namespace ConsoleClient;

public class ChatsControl
{
    string url = "http://localhost:5205/home/";

    public async Task<Chat> GetChatAsync()
    {
        Console.WriteLine("Доступные чаты");

        var chats = GetAll().Result;
        chats.ForEach(u => Console.WriteLine(u.Name.ToString()));
        Chat chat = null;

        while (chat == null)
        {
            Console.WriteLine("Выбери чат");
            var chatName = Console.ReadLine();
            chat = chats.FirstOrDefault(c => c.Name == chatName);
        }

        return chat;
    }

    public async Task<List<Chat>> GetAll()
    {
        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(url);
        var response = httpClient.GetAsync(url + "GetAllChats").Result;
        List<Chat> chats = await response.Content.ReadFromJsonAsync<List<Chat>>();
        return chats;
    }
}