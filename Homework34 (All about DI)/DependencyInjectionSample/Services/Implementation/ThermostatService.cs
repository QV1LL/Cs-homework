using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class ThermostatService : IThermostat
{
    public void SetTemperature(int temperature)
    {
        Console.WriteLine($"Temperature setted up to: {temperature}");
    }
}