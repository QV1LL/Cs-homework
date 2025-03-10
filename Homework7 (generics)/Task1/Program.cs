using Task1To6.Shared;

namespace Task1;

internal static class Program
{
    static void Main(string[] args)
    {
        // Is fibonacci number
        Console.WriteLine(4.IsFibonacci());

        // Get count of words
        Console.WriteLine("Some text for test".GetCountOfWords());

        // Get len of last word
        Console.WriteLine("Some text for test".GetLengthOfLastWords());

        // Check if exists substring in text
        Console.WriteLine("Some text for test".CheckForSubstring("text"));

        // Check correctless of brackets
        Console.WriteLine("({[]})".IsValidBrackets());
    }
}
