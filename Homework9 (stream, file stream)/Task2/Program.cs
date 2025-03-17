namespace Task2;

internal static class Program
{
    private static string _filePath = "text.txt";

    public static void Main(string[] args)
    {
        string? text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), _filePath));
        
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), _filePath), ReplaceWordInText(text, "the", "***"));
    }

    private static string? ReplaceWordInText(string? text, string targetWord, string replaceWord)
    {
        if (text == null) return text;

        int occurence = text.Split(" ").Count(w => w == targetWord);
        Console.WriteLine("Count of replaced words in text: " + occurence);

        return text.Replace(targetWord, replaceWord);
    }
}
