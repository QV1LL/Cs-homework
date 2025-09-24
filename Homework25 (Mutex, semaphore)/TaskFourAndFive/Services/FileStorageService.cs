namespace TaskFourAndFive.Services;

internal static class FileStorageService
{
    public static async Task WriteNumbersToFile(IEnumerable<int> numbers, string relativeFilePath)
    {
        var stringToWrite = string.Join('\n', numbers);
        await File.WriteAllTextAsync(relativeFilePath, stringToWrite);
    }

    public static async Task<IEnumerable<int>> ParseNumbersFromFile(string relativeFilePath)
    {
        var text = await File.ReadAllTextAsync(relativeFilePath);

        if (string.IsNullOrEmpty(text)) return [];

        try
        {
            return text.Split("\n").Select(n => Convert.ToInt32(n));
        }
        catch { return []; }
    }
}
