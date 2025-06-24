using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesApp.Infrastructure.Persistence;

public class GamesAppContext : DbContext
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Studio> Studios => Set<Studio>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=GamesApp.sqlite;Pooling=true;");
    }
}
