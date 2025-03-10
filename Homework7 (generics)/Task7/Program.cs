using Task7.ValueObjects;

namespace Task7;

internal static class Program
{
    static void Main(string[] args)
    {
        Random random = new();
        DayTemperature[] temperatures = new DayTemperature[10];

        for (int i = 0; i < temperatures.Length; i++)
        {
            float minTemp = (float)(random.NextDouble() * 30 + 273);
            float maxTemp = (float)(random.NextDouble() * 20 + minTemp); 

            temperatures[i] = new DayTemperature(maxTemp, minTemp);
        }

        Array.Sort(temperatures, (a, b) => b.Difference.CompareTo(a.Difference));
        Console.WriteLine($"The max difference: {temperatures[0].Difference}");
    }
}
