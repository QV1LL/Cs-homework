using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class CheckoutService(IPaymentProcessor paymentProcessor)
{
    public void Checkout()
    {
        paymentProcessor.Checkout();
    }
}