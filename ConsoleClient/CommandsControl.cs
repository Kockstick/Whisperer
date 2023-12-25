using System.Net.Http.Json;
using ConsoleClient.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClient;

public class CommandsControl
{
    string url = "http://localhost:5205/chat/";
    string homeUrl = "http://localhost:5205/home/";

    private User user;
    private HubConnection connection;

    public delegate void OnUpdateChats_EventHalder(List<Chat> chats);
    public OnUpdateChats_EventHalder OnUpdateChats;

    public CommandsControl(User user)
    {
        this.user = user;
    }

    public bool Command(string text)
    {
        string[] cmd = text.Split(' ');
        switch (cmd[0])
        {
            case "/help":
                PrintTutorial();
                return true;

            case "/exit":
                Disconnect(user.CurrentChat.Name);
                user.CurrentChat = null;
                return true;

            case "/chats":
                UpdateChats();
                return true;

            case "/create":
                if (cmd.Length != 2)
                    return true;
                CreateChat(cmd[1]);
                return true;

            default:
                return false;
        }
    }

    public async Task UpdateChats()
    {
        var chats = await GetAllChats();
        if (chats == null || chats.Count == 0)
        {
            Console.WriteLine("Нет доступных чатов");
            return;
        }
        Console.WriteLine("Доступные чаты");
        chats.ForEach(u => Console.WriteLine(u.Name.ToString()));
        OnUpdateChats?.Invoke(chats);
    }

    public async Task<List<Chat>> GetAllChats()
    {
        try
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(homeUrl);
            var response = await httpClient.PostAsJsonAsync(homeUrl + "GetAllChats", user);
            if (!response.IsSuccessStatusCode)
                return null;
            var chats = await response.Content.ReadFromJsonAsync<List<Chat>>();
            return chats;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + "\n\nInner:  " + ex.InnerException?.Message);
        }

        return null;
    }

    private async void CreateChat(string chatName)
    {
        Chat chat = new Chat()
        {
            Name = chatName
        };

        HttpClient httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(homeUrl);

        UsersChats usersChats = new UsersChats()
        {
            UserId = user.Id,
            Chat = chat,
            RootId = 1
        };

        await httpClient.PostAsJsonAsync(homeUrl + "CreateChat", usersChats);

        UpdateChats();
    }

    public async Task Connect(HubConnection conn, string chatName)
    {
        connection = conn;
        if (connection == null)
            return;
        await connection.InvokeAsync("JoinAsync", chatName);
    }

    public async Task Disconnect(string chatName)
    {
        if (connection == null)
            return;
        await connection.InvokeAsync("LeaveAsync", chatName);
    }

    private void PrintTutorial()
    {
        var text = "\n\nCommands\n" +
            "/exit - Выход\n" +
            "/chats - Список чатов\n" +
            "/create [name] - Создать чат\n" +
            "/delete [name] - Удалить чат\n" +
            "\n\n";

        Console.WriteLine(text);
    }
}