using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamesApp.Infrastructure.Persistence.Configuration;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(g => g.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(g => g.Description)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(g => g.CountOfSales)
               .IsRequired()
               .HasColumnName("CountOfSales");

        builder.Property(g => g.Type)
               .IsRequired()
               .HasColumnName("GameType");

        builder.HasOne(g => g.Studio)
               .WithMany(s => s.Games)
               .HasForeignKey(g => g.StudioId);

        builder.HasMany(g => g.Genres)
               .WithMany(g => g.Games)
               .UsingEntity("GenresGames");
    }
}
