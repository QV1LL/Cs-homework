namespace Task10.ValueObjects;

internal record class ElectricCar : Car
{
    public float BatteryPower
    {
        get => field;
        init
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException("Battery power must be from 0 to 100 percents");

            field = value;
        }
    }

    public ElectricCar(string brand, string model, int realiseYear, float batteryPower)
        : base(brand, model, realiseYear)
    {
        BatteryPower = batteryPower;
    }
}
