using DataAudit.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAudit.Persistence.Configurations;

internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.Property(a => a.EntityName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.Action)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(a => a.ChangedBy)
               .IsRequired()
               .HasMaxLength(100);
    }
}
