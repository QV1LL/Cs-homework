using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GamesApp.Infrastructure.Persistence.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(g => g.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(g => g.Games)
               .WithMany(g => g.Genres)
               .UsingEntity("GenresGames");
    }
}
