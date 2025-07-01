using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GamesApp.Infrastructure.Persistence.Configuration;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(c => c.Country)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(c => c.Studios)
               .WithMany(s => s.Cities)
               .UsingEntity(j => j.ToTable("CitiesStudios"));
    }
}
