namespace Task2;

internal static class Program
{
    public static void Main(string[] args)
    {
        TestActionDelegate();
        TestFuncDelegate();
    }

    private static void TestActionDelegate()
    {
        Action printTimeStats = () => Console.WriteLine("Current time: " + DateTime.Now.ToString("t"));
        printTimeStats += () => Console.WriteLine("Current date: " + DateTime.Now.ToString("d"));
        printTimeStats += () => Console.WriteLine("Current day of week: " + DateTime.Now.DayOfWeek);

        printTimeStats();
    }

    private static void TestFuncDelegate()
    {
        CalculateAndPrintSquare calculateAndPrintSquare = (float a, float b) => Console.WriteLine($"Square of triangle is: {a * b}");
        calculateAndPrintSquare += (float a, float b) => Console.WriteLine($"Square of rectangle is: {a * b}");

        calculateAndPrintSquare(10, 15);
    }
}
