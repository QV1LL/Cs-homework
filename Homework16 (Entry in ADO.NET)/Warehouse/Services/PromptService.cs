using Spectre.Console;
using Warehouse.Common;
using Warehouse.Entities;

namespace Warehouse.Services;

internal static class PromptService
{
    internal static string PromptString(string text)
        => AnsiConsole.Prompt(new TextPrompt<string>(text));

    internal static Result<string> PromptTitle()
    {
        try
        {
            var title = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter product [green]title[/]:")
                    .Validate(s => string.IsNullOrWhiteSpace(s)
                        ? ValidationResult.Error("[red]Title cannot be empty[/]")
                        : ValidationResult.Success()));
            return Result<string>.Success(title);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

    internal static Result<string> PromptType()
    {
        try
        {
            var type = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter product [green]type[/]:")
                    .Validate(s => string.IsNullOrWhiteSpace(s)
                        ? ValidationResult.Error("[red]Type cannot be empty[/]")
                        : ValidationResult.Success()));
            return Result<string>.Success(type);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

    internal static Result<Provider> PromptProvider()
    {
        try
        {
            var providers = Provider.GetAll().ToList();
            if (!providers.Any())
                return Result<Provider>.Failure("No providers available. Please add a provider first.");

            var provider = AnsiConsole.Prompt(
                new SelectionPrompt<Provider>()
                    .Title("Select a [blue]provider[/]:")
                    .PageSize(10)
                    .UseConverter(p => $"{p.Title}")
                    .AddChoices(providers));

            return Result<Provider>.Success(provider);
        }
        catch (Exception ex)
        {
            return Result<Provider>.Failure(ex.Message);
        }
    }

    internal static Result<Product> PromptProduct()
    {
        try
        {
            var products = Product.GetAll().ToList();
            if (!products.Any())
                return Result<Product>.Failure("No products available. Please add a provider first.");

            var product = AnsiConsole.Prompt(
                new SelectionPrompt<Product>()
                    .Title("Select a [blue]product[/]:")
                    .PageSize(10)
                    .UseConverter(p => $"{p.Title}, supply date: {p.SupplyDate.ToString("dd.MM.yyyy")}")
                    .AddChoices(products));

            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(ex.Message);
        }
    }

    internal static Result<int> PromptQuantity()
    {
        try
        {
            var quantity = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [yellow]quantity[/]:")
                    .Validate(q => q < 0
                        ? ValidationResult.Error("[red]Quantity must be ≥ 0[/]")
                        : ValidationResult.Success()));
            return Result<int>.Success(quantity);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex.Message);
        }
    }

    internal static Result<decimal> PromptPrice()
    {
        try
        {
            var price = AnsiConsole.Prompt(
                new TextPrompt<decimal>("Enter [yellow]price[/]:")
                    .Validate(p => p < 0
                        ? ValidationResult.Error("[red]Price must be ≥ 0[/]")
                        : ValidationResult.Success()));
            return Result<decimal>.Success(price);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Failure(ex.Message);
        }
    }

    internal static Result<DateTimeOffset> PromptSupplyDate()
    {
        try
        {
            var date = AnsiConsole.Prompt(
                new TextPrompt<DateTime>("Enter [blue]supply date[/] (YYYY-MM-DD):")
                    .Validate(d => d > DateTime.Now
                        ? ValidationResult.Error("[red]Supply date cannot be in the future[/]")
                        : ValidationResult.Success()));
            return Result<DateTimeOffset>.Success(date.ToUniversalTime());
        }
        catch (Exception ex)
        {
            return Result<DateTimeOffset>.Failure(ex.Message);
        }
    }
}