using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class VisaProcessor : IPaymentProcessor
{
    public void Checkout()
    {
        Console.WriteLine("Paying with Visa");
    }
}