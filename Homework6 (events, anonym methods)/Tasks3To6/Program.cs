using System.Collections.Frozen;
using System.Numerics;

namespace Tasks3To6;

internal static class Program
{
    public static void Main(string[] args)
    {
        var myCollection = Enumerable.Range(-100, 200).ToList();

        Console.WriteLine($"count of numbers that multiples to 7: {myCollection.Count(number => number % 7 == 0)}");
        Console.WriteLine($"count of numbers that in range -20 to 20: {myCollection.Count(number => number >= -20 && number <= 20)}");

        PrintFilteredCollection(myCollection, number => number < 0);

        var printOccurenceOfWordInText = (string? text, string? word) =>
        {
            if (text == null)
                Console.WriteLine("Text is empty");

            if (word == null)
                Console.WriteLine("Word is empty");

            var textParts = text.Split(" ").ToList();

            Console.WriteLine($"Occurence of word in text: {textParts.Count(value => value == word)}");
        };

        string text = "-_- -_- -_- /_/ *-* *_* *_*";

        printOccurenceOfWordInText(text, "*_*");
    }

    private static void PrintFilteredCollection<T>(IEnumerable<T> collection, Predicate<T> predicate)
        where T : INumber<T>
    {
        foreach (var item in collection)
            if (predicate(item))
                Console.Write(item + " ");

        Console.WriteLine();
    }
}
