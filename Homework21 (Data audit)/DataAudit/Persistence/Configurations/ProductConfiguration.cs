using DataAudit.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAudit.Persistence.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(80);

        builder.Property(p => p.Description)
               .IsRequired(false)
               .HasMaxLength(300);

        builder.Property(p => p.Price)
               .IsRequired();
    }
}
