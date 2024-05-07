using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebServer.Models;

namespace WebServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public DateTime Date { get; set; }
        public string? Text { get; set; }
        public string? File { get; set; }
        public int? ReplyMessageId { get; set; }

        public Chat Chat { get; set; }
        public User Sender { get; set; }
        public Message? ReplyMessage { get; set; }
    }
}