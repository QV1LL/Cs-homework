using Microsoft.EntityFrameworkCore;
using SpainChampionship.Domain.Entities;

namespace SpainChampionship.Infrastructure.Persistence;

public class SpainChampionshipContext : DbContext
{
    public DbSet<Team> Teams { get; set; }

    public SpainChampionshipContext(DbContextOptions<SpainChampionshipContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=SpainChampionship.sqlite;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>()
                    .ToTable("Teams")
                    .HasKey(t => t.Id);

        modelBuilder.Entity<Team>()
                    .Property(t => t.Id)
                    .HasColumnName("Id")
                    .HasDefaultValue(Guid.NewGuid());

        modelBuilder.Entity<Team>()
                    .Property(t => t.Name)
                    .HasColumnName("Name")
                    .HasMaxLength(50);

        modelBuilder.Entity<Team>()
                    .Property(t => t.City)
                    .HasColumnName("City")
                    .HasMaxLength(50);

        modelBuilder.Entity<Team>()
                    .Property(t => t.CountOfVictories)
                    .HasColumnName("CountOfVictories");

        modelBuilder.Entity<Team>()
                    .Property(t => t.CountOfDefeats)
                    .HasColumnName("CountOfDefeats");

        modelBuilder.Entity<Team>()
                    .Property(t => t.CountOfDraws)
                    .HasColumnName("CountOfDraws");

        modelBuilder.Entity<Team>()
                    .Property(t => t.CountOfGoals)
                    .HasColumnName("CountOfGoals");

        modelBuilder.Entity<Team>()
                    .Property(t => t.CountOfSkippedGoals)
                    .HasColumnName("CountOfSkippedGoals");
    }
}