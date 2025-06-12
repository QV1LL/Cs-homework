using System.Data.SQLite;
using System.Drawing;
using Spectre.Console.Cli;
using Warehouse.Entities;
using Warehouse.Services;
using Warehouse.Common;
using Warehouse.Enums;
using Warehouse.Commands;

namespace Warehouse;

internal static class Program
{
    const string ConnectionString = "Data Source=VegetablesAndFruits.sqlite;Pooling=true;";

    static Program() => Product.ConnectionString = ConnectionString;

    static void Main(string[] args)
    {
        var connectionResult = IsConnectSuccessful();

        if (connectionResult.IsFailure) 
        {
            LayoutRenderService.PrintError($"Error connection to database: {connectionResult.Error}");
            return;
        }

        LayoutRenderService.PrintSuccess($"Success connection to database");

        AddProductsIfDoesntExist();

        var app = new CommandApp();
        app.Configure(config => config.AddCommand<ShowCommand>("show"));
        app.Run(args);
    }

    static void AddProductsIfDoesntExist()
    {
        var existingProducts = Product.GetAll();
        if (existingProducts.Any()) return;

        var sampleProducts = new List<Product>
        {
            new Product { Title = "Apple", Type = ProductType.Fruit, CalorieContent = 52, Color = Color.Red },
            new Product { Title = "Banana", Type = ProductType.Fruit, CalorieContent = 96, Color = Color.Yellow },
            new Product { Title = "Carrot", Type = ProductType.Vegetable, CalorieContent = 41, Color = Color.Orange },
            new Product { Title = "Spinach", Type = ProductType.Vegetable, CalorieContent = 23, Color = Color.Green },
            new Product { Title = "Blueberry", Type = ProductType.Fruit, CalorieContent = 57, Color = Color.Blue },
            new Product { Title = "Tomato", Type = ProductType.Vegetable, CalorieContent = 18, Color = Color.Red },
            new Product { Title = "Cucumber", Type = ProductType.Vegetable, CalorieContent = 16, Color = Color.Green },
            new Product { Title = "Grape", Type = ProductType.Fruit, CalorieContent = 69, Color = Color.Purple },
            new Product { Title = "Orange", Type = ProductType.Fruit, CalorieContent = 47, Color = Color.Orange },
            new Product { Title = "Potato", Type = ProductType.Vegetable, CalorieContent = 77, Color = Color.Brown },
            new Product { Title = "Strawberry", Type = ProductType.Fruit, CalorieContent = 33, Color = Color.Red },
            new Product { Title = "Lettuce", Type = ProductType.Vegetable, CalorieContent = 15, Color = Color.LightGreen },
            new Product { Title = "Pineapple", Type = ProductType.Fruit, CalorieContent = 50, Color = Color.Goldenrod },
            new Product { Title = "Broccoli", Type = ProductType.Vegetable, CalorieContent = 34, Color = Color.ForestGreen },
            new Product { Title = "Pear", Type = ProductType.Fruit, CalorieContent = 57, Color = Color.LightGreen }
        };

        foreach (var product in sampleProducts) product.Save();
    }

    static void DeleteAllProducts()
    {
        foreach (var product in Product.GetAll()) product.Delete();
    }

    static Result IsConnectSuccessful()
    {
        try
        {
            new SQLiteConnection(ConnectionString).Open();
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }
}
