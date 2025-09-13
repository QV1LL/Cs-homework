namespace Calculator;

internal static class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: <num1> <num2> <operation>");
            return;
        }

        if (!int.TryParse(args[0], out int a) ||
            !int.TryParse(args[2], out int b))
        {
            Console.WriteLine("Error: arguments must be integers");
            return;
        }

        string op = args[1];
        int result = op switch
        {
            "+" => a + b,
            "-" => a - b,
            "*" => a * b,
            "/" => b != 0 ? a / b : 0,
            _ => 0
        };

        Console.WriteLine($"Args: {a} {op} {b}");
        Console.WriteLine($"Result: {result}");
    }
}
