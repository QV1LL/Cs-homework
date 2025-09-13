namespace WordsFinder;

internal static class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: WordsFinder <filePath> <word>");
            return;
        }

        string filePath = args[0];
        string wordToFind = args[1];

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return;
        }

        try
        {
            string content = File.ReadAllText(filePath);
            int count = CountWordOccurrences(content, wordToFind);
            Console.WriteLine($"The word '{wordToFind}' appears {count} time(s) in the file.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading file: {ex.Message}");
        }
    }

    static int CountWordOccurrences(string text, string word)
    {
        int count = 0;
        int index = 0;

        while ((index = text.IndexOf(word, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            count++;
            index += word.Length;
        }

        return count;
    }
}
