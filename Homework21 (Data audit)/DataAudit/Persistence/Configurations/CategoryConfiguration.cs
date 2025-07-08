using DataAudit.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAudit.Persistence.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(80);
    }
}
