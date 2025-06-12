using Spectre.Console;
using Warehouse.Entities;

namespace Warehouse.Services;

internal static class LayoutRenderService
{
    public static void RenderList(string header, string[] items)
    {
        var markup = new List<string>();
        markup.AddRange(items.Select(item => $"- {item}"));

        var list = new Panel(new Markup(string.Join("\n", markup)))
        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader(header)
        };

        AnsiConsole.Write(list);
    }

    public static void RenderProducts(IEnumerable<Product> products)
    {
        var table = new Table().Border(TableBorder.Horizontal);
        table.AddColumn("Title");
        table.AddColumn("Type");
        table.AddColumn("Provider");
        table.AddColumn("Qty");
        table.AddColumn("Price");
        table.AddColumn("Supply date");

        foreach (var p in products)
            table.AddRow(
                p.Title,
                p.Type,
                p.Provider?.Title ?? "[grey]N/A[/]",
                p.Quantity.ToString(),
                $"${p.Price:F2}",
                p.SupplyDate.ToString("dd.MM.yyyy"));

        AnsiConsole.Write(table);
    }

    public static void RenderProviders(IEnumerable<Provider> providers)
    {
        var table = new Table().Border(TableBorder.Horizontal);
        table.AddColumn("Provider Name");

        foreach (var provider in providers)
            table.AddRow(provider.Title);

        AnsiConsole.Write(table);
    }

    public static void PrintInfo(string message) => AnsiConsole.MarkupLine($"[italic blue]{message}[/]");
    public static void PrintError(string message) => AnsiConsole.MarkupLine($"[bold red]{message}[/]");
    public static void PrintSuccess(string message) => AnsiConsole.MarkupLine($"[bold green]{message}[/]");
}
