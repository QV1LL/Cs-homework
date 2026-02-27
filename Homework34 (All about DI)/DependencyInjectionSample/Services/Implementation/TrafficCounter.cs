using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class TrafficCounter : ITrafficCounter
{
    public int Count { get; private set; } = 0;

    public void IncreaseCount(int count)
    {
        Count++;
    }
}