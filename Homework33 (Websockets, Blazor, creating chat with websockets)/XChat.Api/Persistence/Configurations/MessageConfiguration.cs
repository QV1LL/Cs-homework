using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XChat.Api.Models;

namespace XChat.Api.Persistence.Configurations;

internal class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Text)
               .IsRequired()
               .HasMaxLength(500);

        builder.HasOne(e => e.User)
               .WithMany(e => e.Messages)
               .HasForeignKey(e => e.UserId);

        builder.Property(e => e.CreatedAt)
               .ValueGeneratedOnAdd();
    }
}
