using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XChat.Api.Models;

namespace XChat.Api.Persistence.Configurations;

internal class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .HasMaxLength(60)
               .IsRequired();

        builder.Property(e => e.Type)
               .IsRequired();

        builder.HasMany(e => e.Users)
               .WithMany()
               .UsingEntity("RoomsUsers");

        builder.HasMany(e => e.Messages)
               .WithMany()
               .UsingEntity("RoomsMessages");
    }
}
