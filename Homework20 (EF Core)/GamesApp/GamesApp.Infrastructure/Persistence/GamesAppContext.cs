using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GamesApp.Infrastructure.Persistence;

public class GamesAppContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<City> Cities => Set<City>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Studio> Studios => Set<Studio>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GamesAppContext).Assembly);
    }
}
