using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class SecurityService : ISecuritySystem
{
    public void TurnOn()
    {
        Console.WriteLine("Security is turned on!");
    }
}