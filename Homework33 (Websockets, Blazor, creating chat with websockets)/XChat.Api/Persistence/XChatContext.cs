using Microsoft.EntityFrameworkCore;
using XChat.Api.Models;

namespace XChat.Api.Persistence;

internal class XChatContext(DbContextOptions<XChatContext> options) : DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Room> Rooms => Set<Room>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(XChatContext).Assembly);
    }
}
