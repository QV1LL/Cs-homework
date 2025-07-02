using GamesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamesApp.Infrastructure.Persistence.Configuration;

public class StudioConfiguration : IEntityTypeConfiguration<Studio>
{
    public void Configure(EntityTypeBuilder<Studio> builder)
    {
        builder.ToTable("Studios");

        builder.HasKey(s => s.Id);

        builder.Property(c => c.Id)
               .ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(s => s.Games)
               .WithOne(g => g.Studio)
               .HasForeignKey(g => g.StudioId);

        builder.HasMany(s => s.Cities)
               .WithMany(c => c.Studios)
               .UsingEntity(j => j.ToTable("CitiesStudios"));
    }
}
