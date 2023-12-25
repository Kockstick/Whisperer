using Microsoft.EntityFrameworkCore;
using WebServer.Models;

namespace WebServer.Data;

public class ServerDbContext : DbContext
{
    public DbSet<Root> Roots { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<UsersChats> UsersChats { get; set; }

    public ServerDbContext(DbContextOptions<ServerDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .ToTable("User")
            .HasMany(u => u.UsersChats)
            .WithOne(u => u.User)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<Chat>()
            .ToTable("Chat")
            .HasMany(u => u.UsersChats)
            .WithOne(c => c.Chat)
            .HasForeignKey(f => f.ChatId);
    }
}
