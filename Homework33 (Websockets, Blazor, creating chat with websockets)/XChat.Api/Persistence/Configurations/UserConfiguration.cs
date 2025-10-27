using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XChat.Api.Models;

namespace XChat.Api.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.PasswordHash)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(e => e.Messages)
               .WithOne(e => e.User);
    }
}
