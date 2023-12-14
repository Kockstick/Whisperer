using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Models
{
    public class Message
    {
        public int Id {get;set;}
        public int ChatId {get;set;}
        public int SenderId{get;set;}
        public DateTime Date {get;set;}
        public string Text {get;set;}
        public byte[] File {get;set;}
        public int ReplyMessageId{get;set;}

        public User Sender{get;set;}
        public Message ReplyMessage{get;set;}
    }
}