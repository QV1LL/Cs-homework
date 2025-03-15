using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using Task1.Entities;

namespace Task1;

internal static class Program
{
    static void Main(string[] args)
    {
        var plays = new List<Play>();

        for (int i = 0; i < 10; i++)
        {
            using var play = new Play
                (
                    "Some play",
                    new PersonName("Some", "Person"),
                    PlayGenre.Experimental,
                    2025
                );

            plays.Add(play);

            Console.WriteLine($"Plays count: {Play.AlivedObjectsCount}");
            Console.WriteLine($"Names count: {PersonName.AlivedObjectsCount}");
        }

        plays.Clear();
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine($"Plays count: {Play.AlivedObjectsCount}");
        Console.WriteLine($"Names count: {PersonName.AlivedObjectsCount}");
    }
}
