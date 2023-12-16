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
        public DbSet<Root> Roots { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UsersChats> UsersChats { get; set; }

        private string connString = "Server=localhost;Port=3306;Database=whispererdb;UID=root;PWD=Qwerty1#;";
        //Include Error Detail=True

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connString, new MySqlServerVersion(new Version(8, 2, 0)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chat>()
                .ToTable("Chat");
            modelBuilder.Entity<Message>()
                .ToTable("Message");
        }
    }
}