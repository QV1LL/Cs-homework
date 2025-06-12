using Spectre.Console;
using Spectre.Console.Cli;
using Warehouse.Entities;
using Warehouse.Services;

namespace Warehouse.Commands;

internal sealed class ShowCommand : Command<ShowCommand.Settings>
{
    internal class Settings : CommandSettings
    {
        [CommandArgument(0, "<filtering>")]
        public string Report { get; set; }

        [CommandOption("-c|--condition <FIELDNAME>")]
        public string? Condition { get; set; }
        [CommandOption("-q|--min-quantity <QUANTITY>")]
        public int? Quantity { get; set; }

        [CommandOption("-p|--min-price <PRICE>")]
        public decimal? Price { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        switch (settings.Report.ToLower())
        {
            case "products-all":
                {
                    var filteredProducts = Product.GetAll();

                    if (settings.Quantity.HasValue)
                        filteredProducts = filteredProducts.Where(p => p.Quantity >= settings.Quantity.Value);

                    if (settings.Price.HasValue)
                        filteredProducts = filteredProducts.Where(p => p.Price >= settings.Price.Value);

                    if (!filteredProducts.Any())
                    {
                        LayoutRenderService.PrintInfo("No matching products found.");
                        break;
                    }

                    LayoutRenderService.RenderProducts(filteredProducts);
                    break;
                }

            case "providers-all":
                {
                    var providers = Provider.GetAll();

                    if (!providers.Any())
                    {
                        LayoutRenderService.PrintInfo("No providers found.");
                        break;
                    }

                    LayoutRenderService.RenderProviders(providers);
                    break;
                }

            case "types-all":
                {
                    var types = Product.GetAll()
                                       .Select(p => p.Type)
                                       .Distinct()
                                       .ToList();

                    if (!types.Any())
                    {
                        LayoutRenderService.PrintInfo("No product types found.");
                        break;
                    }

                    LayoutRenderService.RenderList("Product Types", types.ToArray());
                    break;
                }

            case "product-with-min":
                {
                    var minProduct = Product.GetAll();

                    if (settings.Condition != "price")
                    {
                        if (settings.Condition != "quantity")
                        {
                            LayoutRenderService.PrintError("Condition must be 'price' or 'quantity'");
                            break;
                        }
                        else minProduct = minProduct.OrderBy(p => p.Quantity);
                    }
                    else minProduct = minProduct.OrderBy(p => p.Price);

                    minProduct = minProduct.Take(1);

                    if (!minProduct.Any())
                    {
                        LayoutRenderService.PrintInfo("No products found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Product with minimum {settings.Condition}:");
                    LayoutRenderService.RenderProducts(minProduct);
                    break;
                }

            case "product-with-max":
                {
                    var minProduct = Product.GetAll();

                    if (settings.Condition != "price")
                    {
                        if (settings.Condition != "quantity")
                            LayoutRenderService.PrintError("Condition must be 'price' or 'quantity'");
                        else minProduct = minProduct.OrderByDescending(p => p.Quantity);
                    }
                    else minProduct = minProduct.OrderByDescending(p => p.Price);

                    minProduct = minProduct.Take(1);

                    if (!minProduct.Any())
                    {
                        LayoutRenderService.PrintInfo("No products found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Product with max {settings.Condition}:");
                    LayoutRenderService.RenderProducts(minProduct);
                    break;
                }

            case "products-by-type":
                {
                    var type = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Select a product [blue]type[/]:")
                            .PageSize(10)
                            .AddChoices(Product.GetAll().Select(p => p.Type).Distinct()));

                    var productsByType = Product.GetAll()
                        .Where(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    LayoutRenderService.PrintInfo($"Products of type '{type}':");
                    LayoutRenderService.RenderProducts(productsByType);
                    break;
                }

            case "products-by-provider":
                {
                    var providerResult = PromptService.PromptProvider();

                    if (providerResult.IsFailure)
                    {
                        LayoutRenderService.PrintError($"No providers were choosen, error: '{providerResult.Error}'.");
                        break;
                    }

                    var productsByProvider = Product.GetAll()
                                                    .Where(p => p.Provider.Id == providerResult.Value!.Id)
                                                    .ToList();

                    if (!productsByProvider.Any())
                    {
                        LayoutRenderService.PrintInfo($"No products found for provider '{providerResult.Value!.Title}'.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Products from provider '{providerResult.Value!.Title}':");
                    LayoutRenderService.RenderProducts(productsByProvider);
                    break;
                }

            case "oldest-product":
                {
                    var oldest = Product.GetAll()
                        .OrderBy(p => p.SupplyDate)
                        .FirstOrDefault();

                    if (oldest is null)
                    {
                        LayoutRenderService.PrintInfo("No products found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo("Oldest product in warehouse:");
                    LayoutRenderService.RenderProducts(new[] { oldest });
                    break;
                }

            case "average-quantity-by-type":
                {
                    var grouped = Product.GetAll()
                        .GroupBy(p => p.Type)
                        .Select(g => new { Type = g.Key, AvgQuantity = g.Average(p => p.Quantity) })
                        .ToList();

                    if (!grouped.Any())
                    {
                        LayoutRenderService.PrintInfo("No product types found.");
                        break;
                    }

                    var table = new Table();
                    table.AddColumn("Product Type");
                    table.AddColumn("Average Quantity");

                    foreach (var item in grouped)
                        table.AddRow(item.Type, item.AvgQuantity.ToString("0.##"));

                    AnsiConsole.Write(table);
                    break;
                }

            case "provider-with-most":
                {
                    var grouped = Product.GetAll()
                        .GroupBy(p => p.Provider)
                        .Select(g => new { Provider = g.Key, Total = g.Sum(p => p.Quantity) })
                        .OrderByDescending(g => g.Total)
                        .FirstOrDefault();

                    if (grouped is null)
                    {
                        LayoutRenderService.PrintInfo("No providers found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Provider with the most items in stock: {grouped.Provider} ({grouped.Total} units)");
                    var providers = Provider.GetAll()
                                           .Where(p => p.Id == grouped.Provider.Id)
                                           .Take(1);
                    if (providers != null)
                        LayoutRenderService.RenderProviders(providers);
                    break;
                }

            case "provider-with-least":
                {
                    var grouped = Product.GetAll()
                        .GroupBy(p => p.Provider)
                        .Select(g => new { Provider = g.Key, Total = g.Sum(p => p.Quantity) })
                        .OrderBy(g => g.Total)
                        .FirstOrDefault();

                    if (grouped is null)
                    {
                        LayoutRenderService.PrintInfo("No providers found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Provider with the least items in stock: {grouped.Provider} ({grouped.Total} units)");
                    var providers = Provider.GetAll()
                                          .Where(p => p.Id == grouped.Provider.Id)
                                          .Take(1);
                    if (providers != null)
                        LayoutRenderService.RenderProviders(providers);
                    break;
                }

            case "type-with-most":
                {
                    var grouped = Product.GetAll()
                        .GroupBy(p => p.Type)
                        .Select(g => new { Type = g.Key, Total = g.Sum(p => p.Quantity) })
                        .OrderByDescending(g => g.Total)
                        .FirstOrDefault();

                    if (grouped is null)
                    {
                        LayoutRenderService.PrintInfo("No product types found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Product type with most units in stock: {grouped.Type} ({grouped.Total} units)");
                    break;
                }

            case "type-with-least":
                {
                    var grouped = Product.GetAll()
                        .GroupBy(p => p.Type)
                        .Select(g => new { Type = g.Key, Total = g.Sum(p => p.Quantity) })
                        .OrderBy(g => g.Total)
                        .FirstOrDefault();

                    if (grouped is null)
                    {
                        LayoutRenderService.PrintInfo("No product types found.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Product type with least units in stock: {grouped.Type} ({grouped.Total} units)");
                    break;
                }

            case "products-older-than":
                {
                    if (!int.TryParse(settings.Condition, out int days))
                    {
                        LayoutRenderService.PrintError("Please provide the number of days using --condition <DAYS>.");
                        break;
                    }

                    var threshold = DateTime.Now.AddDays(-days);
                    var oldProducts = Product.GetAll()
                        .Where(p => p.SupplyDate < threshold)
                        .ToList();

                    if (!oldProducts.Any())
                    {
                        LayoutRenderService.PrintInfo($"No products found that were supplied more than {days} days ago.");
                        break;
                    }

                    LayoutRenderService.PrintInfo($"Products supplied more than {days} days ago:");
                    LayoutRenderService.RenderProducts(oldProducts);
                    break;
                }

            default:
                LayoutRenderService.PrintError("Unknown report type. Options: [products-all, providers-all, types-all, product-with-min, product-with-max, products-by-type, products-by-provider, oldest-product, average-quantity-by-type]");
                break;
        }

        return 0;
    }
}
