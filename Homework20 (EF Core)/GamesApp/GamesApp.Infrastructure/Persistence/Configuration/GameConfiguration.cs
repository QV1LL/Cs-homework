﻿using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GamesApp.Infrastructure.Persistence.Configuration;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.ToTable("Games");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
               .HasDefaultValue(Guid.NewGuid());

        builder.Property(g => g.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(g => g.Description)
               .IsRequired()
               .HasMaxLength(1000);

        builder.Property(g => g.GameType)
               .IsRequired();

        builder.HasOne(g => g.Studio)
               .WithMany(s => s.Games)
               .HasForeignKey(g => g.StudioId);

        builder.HasMany(g => g.Genres);
    }
}
