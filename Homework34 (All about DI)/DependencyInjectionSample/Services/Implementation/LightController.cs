using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class LightController : ILightController
{
    public void SetBrightness(int lumens)
    {
        Console.WriteLine($"Brightness: {lumens}");
    }
}