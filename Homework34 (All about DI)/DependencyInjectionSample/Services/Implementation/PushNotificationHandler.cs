using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class PushNotificationHandler : INotificationHandler
{
    public void Send(string message)
    {
        Console.WriteLine($"Pushed notification: {message}");
    }
}