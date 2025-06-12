using Spectre.Console;
using Spectre.Console.Cli;
using Warehouse.Entities;
using Warehouse.Enums;
using Warehouse.Services;

namespace Warehouse.Commands;

internal class ShowCommand : Command<ShowCommand.Settings>
{
    internal class Settings : CommandSettings
    {
        [CommandOption("-f|--filter <FILTEROPTION>")]
        public string? Filter { get; set; }

        [CommandOption("-c|--calorie <CALORIEOPTION>")]
        public int? Calorie { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var allProducts = Product.GetAll();
        var filter = settings.Filter?.ToLower();

        if (filter is null)
        {
            LayoutRenderService.RenderProducts(allProducts);
            return 0;
        }

        IEnumerable<Product> filtered;
        switch (filter)
        {
            case "titles":
                var titles = allProducts.Select(p => p.Title).ToArray();
                LayoutRenderService.RenderList("Product Titles", titles);
                break;

            case "colors":
                var colors = allProducts.Select(p => p.Color.Name).Distinct().ToArray();
                LayoutRenderService.RenderList("Product Colors", colors);
                break;

            case "max-calorie":
                var max = allProducts.Max(p => p.CalorieContent);
                LayoutRenderService.PrintInfo($"Maximum calorie content: {max}");
                break;

            case "min-calorie":
                var min = allProducts.Min(p => p.CalorieContent);
                LayoutRenderService.PrintInfo($"Minimum calorie content: {min}");
                break;

            case "avg-calorie":
                var avg = allProducts.Average(p => p.CalorieContent);
                LayoutRenderService.PrintInfo($"Average calorie content: {avg:F2}");
                break;

            case "vegetables-count":
                var vegCount = allProducts.Count(p => p.Type == ProductType.Vegetable);
                LayoutRenderService.PrintInfo($"Vegetables: {vegCount}");
                break;

            case "fruits-count":
                var fruitCount = allProducts.Count(p => p.Type == ProductType.Fruit);
                LayoutRenderService.PrintInfo($"Fruits: {fruitCount}");
                break;

            case "product-count-of-color":
                var targetColor = PromptService.PromptColor().Value;
                var count = allProducts.Count(p => p.Color.Equals(targetColor));
                LayoutRenderService.PrintInfo($"Products with color {targetColor.Name}: {count}");
                break;

            case "product-count-of-every-color":
                var grouped = allProducts.GroupBy(p => p.Color.Name);
                foreach (var group in grouped)
                    LayoutRenderService.PrintInfo($"{group.Key}: {group.Count()}");
                break;

            case "below-calorie":
                if (settings.Calorie is not int belowVal)
                {
                    LayoutRenderService.PrintError("Provide calorie with --calorie option.");
                    break;
                }
                filtered = allProducts.Where(p => p.CalorieContent < belowVal);
                LayoutRenderService.RenderProducts(filtered);
                break;

            case "above-calorie":
                if (settings.Calorie is not int aboveVal)
                {
                    LayoutRenderService.PrintError("Provide calorie with --calorie option.");
                    break;
                }
                filtered = allProducts.Where(p => p.CalorieContent > aboveVal);
                LayoutRenderService.RenderProducts(filtered);
                break;

            case "between-calorie":
                var minCal = AnsiConsole.Ask<int>("Enter minimum calorie:");
                var maxCal = AnsiConsole.Ask<int>("Enter maximum calorie:");
                filtered = allProducts.Where(p => p.CalorieContent >= minCal && p.CalorieContent <= maxCal);
                LayoutRenderService.RenderProducts(filtered);
                break;

            case "yellow-or-red":
                filtered = allProducts.Where(p =>
                    p.Color.Equals(Color.Yellow) || p.Color.Equals(Color.Red));
                LayoutRenderService.RenderProducts(filtered);
                break;

            default:
                LayoutRenderService.PrintError("Unknown filter option.");
                break;
        }

        return 0;
    }
}
