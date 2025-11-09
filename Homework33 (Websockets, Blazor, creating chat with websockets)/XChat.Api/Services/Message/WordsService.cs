using System.Text.RegularExpressions;

namespace XChat.Api.Services.Message;

internal class WordsService
{
    private readonly HashSet<string> _profanityWords = new(StringComparer.OrdinalIgnoreCase);

    public WordsService()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "profanity_words.txt");

        if (File.Exists(path))
        {
            foreach (var line in File.ReadAllLines(path))
            {
                var word = line.Trim();
                if (!string.IsNullOrEmpty(word) && !word.StartsWith('#'))
                    _profanityWords.Add(word);
            }
        }
    }

    public bool IsAllowed(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
            return true;

        return !_profanityWords.Contains(word.Trim());
    }

    public string ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        string result = text;

        foreach (var badWord in _profanityWords)
        {
            var pattern = $@"\b{Regex.Escape(badWord)}\b";
            result = Regex.Replace(result, pattern, "###", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        return result;
    }
}
