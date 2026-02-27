namespace DependencyInjectionSample.Services.Contracts;

public interface INotificationHandler
{
    void Send(string message);
}