using Spectre.Console;
using Warehouse.Entities;
using Warehouse.Common;

namespace Warehouse.Services;

internal static class PromptService
{
    public static Result<System.Drawing.Color> PromptColor()
    {
        try
        {
            var color = AnsiConsole.Prompt(
                new SelectionPrompt<System.Drawing.Color>()
                    .Title("Select a [blue]color[/]:")
                    .PageSize(10)
                    .UseConverter(c => $"{c.ToString()}")
                    .AddChoices(Product.GetAll().Select(p => p.Color)));

            return Result<System.Drawing.Color>.Success(color);
        }
        catch (Exception ex)
        {
            return Result<System.Drawing.Color>.Failure(ex.Message);
        }
    }
}