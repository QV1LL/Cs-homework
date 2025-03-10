namespace Task7.ValueObjects;

internal record class DayTemperature(float maxTemperature, float minTemperature)
{
    public float MaxTemperature
    {
        get => maxTemperature;
        init
        {
            if (value < 273)
                throw new ArgumentOutOfRangeException("Uncorrect temperature");

            maxTemperature = value;
        }
    }

    public float MinTemperature
    {
        get => minTemperature;
        init
        {
            if (value < 273)
                throw new ArgumentOutOfRangeException("Uncorrect temperature");

            minTemperature = value;
        }
    }

    public float Difference { get => Math.Abs(maxTemperature - minTemperature); }
}
