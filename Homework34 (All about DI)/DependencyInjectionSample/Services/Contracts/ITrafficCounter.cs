namespace DependencyInjectionSample.Services.Contracts;

public interface ITrafficCounter
{
    public int Count { get; }
    
    void IncreaseCount(int count);
}