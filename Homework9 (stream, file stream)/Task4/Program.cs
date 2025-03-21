namespace Task4;

internal static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter a relative path to file");

        string? input = Console.ReadLine();
        string? fullFilePath = input ?? Path.Combine(Directory.GetCurrentDirectory(), input);

        if (fullFilePath == null || !File.Exists(fullFilePath)) return;

        string reversedText = new string(File.ReadAllText(fullFilePath).Reverse().ToArray());
        File.WriteAllText(Path.GetFileNameWithoutExtension(fullFilePath) + "(reversed)" + Path.GetExtension(fullFilePath), reversedText);
    }
}
