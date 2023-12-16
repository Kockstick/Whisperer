using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Database;
using Server.Models;

namespace Server.Models;

public class Chat
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CreatorId { get; set; }

    public User Creator { get; set; }

    public delegate void OnMessage_EventHalder(Message message);
    public OnMessage_EventHalder OnGetMessage;

    public void SendMessage(Message message)
    {
        using (var context = new WContext())
        {
            context.Messages.Add(message);
            context.SaveChanges();
        }
    }

    public void GetMessage(Message message)
    {
        if (OnGetMessage != null)
            OnGetMessage(message);
    }
}