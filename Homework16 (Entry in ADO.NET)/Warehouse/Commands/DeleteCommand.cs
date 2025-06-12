using Spectre.Console.Cli;
using Warehouse.Common;
using Warehouse.Services;

namespace Warehouse.Commands;

internal sealed class DeleteCommand : Command<DeleteCommand.Settings>
{
    internal class Settings : CommandSettings
    {
        [CommandArgument(0, "<entity>")]
        public string Entity { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var result = settings.Entity.ToLower() switch
        {
            "product" => ProductService.Delete(),
            "provider" => ProviderService.Delete(),
            _ => Result.Failure("Invalid entity! Use: product, provider")
        };

        if (result.IsSuccess)
            LayoutRenderService.PrintSuccess($"{settings.Entity} deleted successfully!");
        else
            LayoutRenderService.PrintError($"{settings.Entity} deletion failed: {result.Error}");
        return result.IsSuccess ? 0 : 1;
    }
}
