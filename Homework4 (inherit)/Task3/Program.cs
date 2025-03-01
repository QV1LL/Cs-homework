using Task3.Entities;

namespace Task3;

internal static class Program
{
    public static void Main(string[] args)
    {
        var instrument = new Violin
        (
            "Paganini`s Violin", 
            "Very expensive violin, that related to one of the famous composer", 
            "History of this instrument begun in 18 century, when..."
        );

        TestInstrument(instrument);
    }

    private static void TestInstrument(MusicalInstrument instrument)
    {
        Console.WriteLine(instrument.Title);
        Console.WriteLine(instrument.Descriprion);
        instrument.PlaySound();
    }
}
