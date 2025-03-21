using Task5.Services;

namespace Task5;

internal static class Program
{
    static void Main(string[] args)
    {
        string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "numbers.txt");
        FileAnalyzer analyzer = new FileAnalyzer(inputFilePath);

        try
        {
            var positiveNumbers = analyzer.GetAndAnalyzeNumbers(n => n > 0);
            analyzer.SaveToFile("positive.txt");

            var negativeNumbers = analyzer.GetAndAnalyzeNumbers(n => n < 0);
            analyzer.SaveToFile("negative.txt");

            var twoDigitNumbers = analyzer.GetAndAnalyzeNumbers(n => (n >= 10 && n <= 99) || (n <= -10 && n >= -99));
            analyzer.SaveToFile("two_digit.txt");

            var fiveDigitNumbers = analyzer.GetAndAnalyzeNumbers(n => (n >= 10000 && n <= 99999) || (n <= -10000 && n >= -99999));
            analyzer.SaveToFile("five_digit.txt");

            Console.WriteLine("Analyze completed, new files were created!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
