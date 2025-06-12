using Spectre.Console.Cli;
using Warehouse.Services;

namespace Warehouse.Commands;

internal sealed class HelpCommand : Command<HelpCommand.Settings>
{
    internal class Settings : CommandSettings
    {

    }

    public override int Execute(CommandContext context, Settings settings)
    {
        LayoutRenderService.RenderList("Available commands", new[]
        {
            "[bold blue]add[/] - Add new entities",
            "[bold blue]update[/] - Update existing entities",
            "[bold blue]delete[/] - Delete entities",
            "[bold blue]show[/] - Display analytics or lists"
        });

        return 0;
    }
}
