namespace Task6;

internal static class Program
{
    static void Main(string[] args)
    {
        Action<int> printNumberStats = (int number) => Console.WriteLine($"Is even: {number % 2 == 0}");
        printNumberStats += (int number) => Console.WriteLine($"Is odd: {number % 2 == 1}");
        printNumberStats += (int number) => Console.WriteLine($"Is positive: {number > 0}");

        printNumberStats(15);
    }
}
