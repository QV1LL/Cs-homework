using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class SmsHandler : INotificationHandler
{
    public void Send(string message)
    {
        Console.WriteLine(message);
    }
}