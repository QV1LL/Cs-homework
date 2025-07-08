using DataAudit.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Text.Json;

namespace DataAudit.Persistence;

internal class DataAuditContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=DataAudit.sqlite;Pooling=true;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataAuditContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(IAuditable).IsAssignableFrom(t.ClrType)))
        {
            modelBuilder.Entity(entityType.Name)
                .Property<DateTime>("CreatedAt")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity(entityType.Name)
                .Property<string>("CreatedBy")
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity(entityType.Name)
                .Property<DateTime?>("UpdatedAt");

            modelBuilder.Entity(entityType.Name)
                .Property<string>("UpdatedBy")
                .HasMaxLength(100);

            modelBuilder.Entity(entityType.Name)
                .Property<DateTime?>("DeletedAt");

            modelBuilder.Entity(entityType.Name)
                .Property<string>("DeletedBy")
                .HasMaxLength(100);

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                new[] { typeof(DateTime?) },
                parameter,
                Expression.Constant("DeletedAt"));
            var nullConstant = Expression.Constant(null, typeof(DateTime?));
            var equals = Expression.Equal(property, nullConstant);
            var lambda = Expression.Lambda(equals, parameter);

            modelBuilder.Entity(entityType.Name)
                .HasQueryFilter(lambda);
        }
    }

    public int SaveChanges(string userName)
    {
        var auditEntries = OnBeforeSaveChanges(userName);
        var result = base.SaveChanges();
        OnAfterSaveChanges(auditEntries);
        return result;
    }

    public async Task<int> SaveChangesAsync(string userName, CancellationToken cancellationToken = default)
    {
        var auditEntries = OnBeforeSaveChanges(userName);
        var result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChangesAsync(auditEntries, cancellationToken);
        return result;
    }

    private List<AuditLog> OnBeforeSaveChanges(string currentUser)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditLog>();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = now;
                entry.Property("CreatedBy").CurrentValue = currentUser;
                auditEntries.Add(new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = GetEntityId(entry),
                    Action = "Create",
                    ChangedBy = currentUser,
                    ChangedAt = now,
                    NewValues = JsonSerializer.Serialize(ToDictionary(entry, true))
                });
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = now;
                entry.Property("UpdatedBy").CurrentValue = currentUser;
                auditEntries.Add(new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = GetEntityId(entry),
                    Action = "Update",
                    ChangedBy = currentUser,
                    ChangedAt = now,
                    OldValues = JsonSerializer.Serialize(ToDictionary(entry, false)),
                    NewValues = JsonSerializer.Serialize(ToDictionary(entry, true))
                });
            }
            else if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Property("DeletedAt").CurrentValue = now;
                entry.Property("DeletedBy").CurrentValue = currentUser;
                auditEntries.Add(new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = GetEntityId(entry),
                    Action = "Delete",
                    ChangedBy = currentUser,
                    ChangedAt = now,
                    OldValues = JsonSerializer.Serialize(ToDictionary(entry, false))
                });
            }
        }

        return auditEntries;
    }

    private void OnAfterSaveChanges(List<AuditLog> auditEntries)
    {
        AuditLogs.AddRange(auditEntries);
        base.SaveChanges();
    }

    private async Task OnAfterSaveChangesAsync(List<AuditLog> auditEntries, CancellationToken cancellationToken)
    {
        await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
        await base.SaveChangesAsync(cancellationToken);
    }

    private static Guid GetEntityId(EntityEntry entry)
    {
        return (Guid)entry.Property("Id").CurrentValue!;
    }

    private static Dictionary<string, object?> ToDictionary(EntityEntry entry, bool useCurrentValues)
    {
        var dict = new Dictionary<string, object?>();
        foreach (var prop in entry.Properties.Where(p => !p.Metadata.IsForeignKey()))
        {
            var value = useCurrentValues ? prop.CurrentValue : prop.OriginalValue;
            dict[prop.Metadata.Name] = value;
        }

        return dict;
    }
}
