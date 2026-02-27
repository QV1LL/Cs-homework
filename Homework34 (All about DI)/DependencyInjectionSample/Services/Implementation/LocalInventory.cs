using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class LocalInventory : IInventoryCheck
{
    public bool IsAvailable() => true;
}