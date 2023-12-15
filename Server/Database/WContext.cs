using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Models;

namespace Server.Database
{
    public class WContext : DbContext
    {
        public DbSet<Root> Roots;
        public DbSet<User> Users;
        public DbSet<Chat> Chats;
        public DbSet<UsersChats> UsersChats;

        private string connString = "Server=localhost;Port=5432;Database=pro9;UID=postgres;PWD=admin;Include Error Detail=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connString);
        }
    }
}