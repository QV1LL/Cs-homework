using NumbersAnalyzer.Services;

namespace NumbersAnalyzer;

internal static class Program
{
    private const string FILE_PATH = "numbers.txt";

    static void Main()
    {
        Console.WriteLine(
            NumbersService.CountUnique(
                FileParseService.ParseNumbers(FILE_PATH)
            )
        );

        Console.WriteLine(
            NumbersService.CountMaxIncreasingSequence(
                FileParseService.ParseNumbers(FILE_PATH)
            )
        );
    }
}
