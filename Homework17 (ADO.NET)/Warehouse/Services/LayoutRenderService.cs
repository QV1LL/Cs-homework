using Spectre.Console;
using System.Globalization;
using System.Drawing;
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
        table.AddColumn("Color");
        table.AddColumn("Calorie Content");

        foreach (var product in products)
            table.AddRow(
                product.Title,
                product.Type.ToString(),
                ColorTranslator.ToHtml(product.Color).ToString(),
                product.CalorieContent.ToString());

        AnsiConsole.Write(table);
    }

    public static void PrintInfo(string message) => AnsiConsole.MarkupLine($"[italic blue]{message}[/]");
    public static void PrintError(string message) => AnsiConsole.MarkupLine($"[bold red]{message}[/]");
    public static void PrintSuccess(string message) => AnsiConsole.MarkupLine($"[bold green]{message}[/]");
}