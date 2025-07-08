using DataAudit.Entities;
using DataAudit.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DataAudit;

internal static class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Testing program!");

        var options = new DbContextOptionsBuilder<DataAuditContext>()
                .EnableSensitiveDataLogging()
                .Options;

        using var context = new DataAuditContext(options);

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var product = new Product { Name = "Laptop", Price = 1000 };
        context.Products.Add(product);
        await context.SaveChangesAsync("user1");
        product.Price = 1200;
        await context.SaveChangesAsync("user2");
        context.Products.Remove(product);
        await context.SaveChangesAsync("user3");
    }
}
