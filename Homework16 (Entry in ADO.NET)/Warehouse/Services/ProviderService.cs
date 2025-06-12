using Warehouse.Common;
using Warehouse.Entities;

namespace Warehouse.Services;

internal static class ProviderService
{
    internal static Result Add()
    {
        var titleResult = PromptService.PromptTitle();

        if (titleResult.IsFailure)
            return Result.Failure(titleResult.Error!);

        new Provider
        {
            Title = titleResult.Value!
        }.Save();

        return Result.Success();
    }

    internal static Result Update()
    {
        var providerToUpdateResult = PromptService.PromptProvider();

        if (providerToUpdateResult.IsFailure) return Result.Failure(providerToUpdateResult.Error!);

        var providerToUpdate = providerToUpdateResult.Value!;
        var titleResult = PromptService.PromptTitle();

        if (titleResult.IsFailure)
            return Result.Failure(titleResult.Error!);

        providerToUpdate.Title = titleResult.Value!;
        providerToUpdate.Save();

        return Result.Success();
    }

    internal static Result Delete()
    {
        var providerToDeleteResult = PromptService.PromptProvider();

        if (providerToDeleteResult.IsFailure) return Result.Failure(providerToDeleteResult.Error!);
        if (Product.GetAll().Any(p => p.Provider.Id == providerToDeleteResult.Value!.Id)) 
            return Result.Failure("Cannot delete provider that used in products!");

        providerToDeleteResult.Value!.Delete();

        return Result.Success();
    }
}
