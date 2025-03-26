namespace Task1;

internal static class Program
{
    static void Main(string[] args)
    {
        var surnames = new string[]
        {
            "Vereshagin",
            "Pyshka",
            "Melenevych",
            "Somesurname",
            "...",
            "-_-",
            "@_@",
        };

        Console.WriteLine($"All is bigger than 3 symbols: {surnames.All(s => s.Length > 3)}");
        Console.WriteLine($"All is bigger than 3 and above 10 symbols: {surnames.All(s => s.Length > 3 && s.Length < 10)}");
        Console.WriteLine($"Any starts with W: {surnames.Any(s => s.StartsWith('W'))}");
        Console.WriteLine($"Any ends with Y: {surnames.Any(s => s.EndsWith('Y'))}");
        Console.WriteLine($"Orange: {surnames.Contains("Orange")}");
        Console.WriteLine($"First of length 6: {surnames.FirstOrDefault(s => s.Length == 6)}");
        Console.WriteLine($"Last with length above 15: {surnames.LastOrDefault(s => s.Length < 15)}");
    }
}
