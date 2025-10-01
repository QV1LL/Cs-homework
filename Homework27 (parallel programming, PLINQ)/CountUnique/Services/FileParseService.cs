namespace NumbersAnalyzer.Services;

internal static class FileParseService
{
    public static IEnumerable<int> ParseNumbers(string relativePath)
    {
        if (!File.Exists(relativePath))
            return [];

        var lines = File.ReadAllLines(relativePath);

        return lines.AsParallel()
                    .Select(l => Convert.ToInt32(l));
    }
}
