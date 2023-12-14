using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Database
{
    public class WContext : DbContext
    {
        public DbSet<Root> Roots;
        public DbSet<User> Users;
        public DbSet<Chat> Chats;
        public DbSet<UsersChats> UsersChats;
    }
}