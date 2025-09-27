using System.Text.RegularExpressions;

namespace TextAnalizer.Services;

internal class TextExclamationSentencesAnalizer : ITextAnalizeService
{
    public int GetCount(string text)
    {
        string pattern = @"\w+.*?!(?=(\s|$))";

        return new Regex(pattern).Matches(text).Count;
    }
}
