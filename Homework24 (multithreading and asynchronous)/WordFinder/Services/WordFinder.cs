using System.IO;
using System.Text.RegularExpressions;

namespace WordFinder.Services;

internal class WordFinder : IWordFinder
{
    public async Task<int> GetCountOfWordAppearenceAsync(string word, string fileAbsolutePath)
    {
        if (string.IsNullOrWhiteSpace(word))
            return 0;
        if (!File.Exists(fileAbsolutePath))
            throw new FileNotFoundException("File not found", fileAbsolutePath);

        string text = await File.ReadAllTextAsync(fileAbsolutePath);

        var pattern = $@"\b{Regex.Escape(word)}\b";
        var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);

        return matches.Count;
    }
}
