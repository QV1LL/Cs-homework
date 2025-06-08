using Warehouse.Entities;

namespace Warehouse;

internal static class Program
{
    private static readonly string ConnectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "Warehouse.sqlite")};Pooling=true;";

    static Program()
    {
        Product.ConnectionString = ConnectionString;
        Provider.ConnectionString = ConnectionString;
    }

    static void Main(string[] args)
    {
        FillDatabaseIfEmpty();
    }

    private static void FillDatabaseIfEmpty()
    {
        if (Provider.GetAll().Any() || Product.GetAll().Any()) return;

        var providers = new[]
        {
            new Provider { Title = "Якийсь фермер" },
            new Provider { Title = "Приватний підприємець" },
            new Provider { Title = "Ще якийсь поставщик)))" }
        };
        foreach (var provider in providers) provider.Save();

        var products = new[]
        {
            new Product { Title = "Молоко", Type = "Продукти", Provider = providers[0], Quantity = 100, Price = 59.99m, SupplyDate = DateTimeOffset.Now },
            new Product { Title = "Хліб", Type = "Продукти", Provider = providers[0], Quantity = 200, Price = 29.50m, SupplyDate = DateTimeOffset.Now },
            new Product { Title = "Кава", Type = "Напої", Provider = providers[1], Quantity = 50, Price = 299.00m, SupplyDate = DateTimeOffset.Now },
            new Product { Title = "Сир", Type = "Продукти", Provider = providers[2], Quantity = 75, Price = 199.99m, SupplyDate = DateTimeOffset.Now },
            new Product { Title = "Чай", Type = "Напої", Provider = providers[1], Quantity = 60, Price = 149.50m, SupplyDate = DateTimeOffset.Now }
        };
        foreach (var product in products) product.Save();
    }
}
