using Spectre.Console.Cli;
using Warehouse.Commands;
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
        var app = new CommandApp();

        app.Configure(config =>
        {
            config.AddCommand<HelpCommand>("help");
            config.AddCommand<AddCommand>("add");
            config.AddCommand<UpdateCommand>("update");
            config.AddCommand<DeleteCommand>("delete");
            config.AddCommand<ShowCommand>("show");
        });

        app.Run(args);
    }
}
