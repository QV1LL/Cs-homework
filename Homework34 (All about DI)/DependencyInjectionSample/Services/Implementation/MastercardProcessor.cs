using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class MastercardProcessor : IPaymentProcessor
{
    public void Checkout()
    {
        Console.WriteLine("Paying with Mastercard");
    }
}