using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Models
{
    public class User
    {
        public int Id {get; set;}
        public string Login {get;set;}
        public string Password {get;set;}
        public string Name {get;set;}
        public int ChatsId {get;set;}

        public UsersChats Chats  {get;set;}
    }
}