using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class SmartHomeManager(IThermostat thermostat, 
                              ILightController lightController, 
                              ISecuritySystem securitySystem)
{
    private const int EveningTemperature = 22;
    
    public void ActivateEveningMode()
    {
        thermostat.SetTemperature(EveningTemperature);
        lightController.SetBrightness(0);
        securitySystem.TurnOn();
    }
}