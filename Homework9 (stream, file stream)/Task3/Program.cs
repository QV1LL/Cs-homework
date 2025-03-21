namespace Task3;

internal static class Program
{
    private static string _filePath;

    static void Main(string[] args)
    {
        string? filePath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(filePath) || filePath == null)
        {
            Console.WriteLine("File path is incorrect");
            return;
        }

        _filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        string[] deprecatedWords = Console.ReadLine()?.Split(" ") ?? new string[]{ string.Empty };

        if (!File.Exists(_filePath)) return;

        string? text = File.ReadAllText(_filePath);

        if (text == null) return;

        foreach (var word in deprecatedWords)
            text = text.Replace(word, string.Concat(Enumerable.Repeat("*", word.Length)));

        File.WriteAllText(_filePath, text);
    }   
}
