using Warehouse.Common;
using Warehouse.Entities;

namespace Warehouse.Services;

internal static class ProductService
{
    internal static Result Add()
    {
        var results = new List<Result>();

        var titleResult = PromptService.PromptTitle();
        results.Add(titleResult);

        var typeResult = PromptService.PromptType();
        results.Add(typeResult);

        var providerResult = PromptService.PromptProvider();
        results.Add(providerResult);

        var priceResult = PromptService.PromptPrice();
        results.Add(priceResult);

        var quantityResult = PromptService.PromptQuantity();
        results.Add(quantityResult);

        if (results.Any(r => r.IsFailure)) 
            return Result.Failure(results.First(r => r.IsFailure)?.Error ?? "Failed to add product");

        new Product
        {
            Title = titleResult.Value!,
            Type = typeResult.Value!,
            Price = priceResult.Value!,
            Quantity = quantityResult.Value!,
            Provider = providerResult.Value!,
        }.Save();

        return Result.Success();
    }

    internal static Result Update()
    {
        var productToUpdateResult = PromptService.PromptProduct();

        if (productToUpdateResult.IsFailure) return Result.Failure(productToUpdateResult.Error!);

        var productToUpdate = productToUpdateResult.Value!;
        var results = new List<Result>();

        var titleResult = PromptService.PromptTitle();
        results.Add(titleResult);

        var typeResult = PromptService.PromptType();
        results.Add(typeResult);

        var providerResult = PromptService.PromptProvider();
        results.Add(providerResult);

        var priceResult = PromptService.PromptPrice();
        results.Add(priceResult);

        var quantityResult = PromptService.PromptQuantity();
        results.Add(quantityResult);

        if (results.Any(r => r.IsFailure))
            return Result.Failure(results.First(r => r.IsFailure)?.Error ?? "Failed to add product");

        productToUpdate.Title = titleResult.Value!;
        productToUpdate.Type = typeResult.Value!;
        productToUpdate.Price = priceResult.Value!;
        productToUpdate.Quantity = quantityResult.Value!;
        productToUpdate.Provider = providerResult.Value!;
        productToUpdate.Save();

        return Result.Success();
    }

    internal static Result Delete()
    {
        var productToDeleteResult = PromptService.PromptProduct();

        if (productToDeleteResult.IsFailure) return Result.Failure(productToDeleteResult.Error!);

        productToDeleteResult.Value!.Delete();

        return Result.Success();
    }
}
