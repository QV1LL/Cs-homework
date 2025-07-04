﻿using Spectre.Console.Cli;
using Warehouse.Common;
using Warehouse.Services;

namespace Warehouse.Commands;

internal sealed class UpdateCommand : Command<UpdateCommand.Settings>
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
            "product" => ProductService.Update(),
            "provider" => ProviderService.Update(),
            _ => Result.Failure("Invalid entity! Use: product, provider")
        };

        if (result.IsSuccess)
            LayoutRenderService.PrintSuccess($"{settings.Entity} updated successfully!");
        else
            LayoutRenderService.PrintError($"{settings.Entity} update failed: {result.Error}");
        return result.IsSuccess ? 0 : 1;
    }
}