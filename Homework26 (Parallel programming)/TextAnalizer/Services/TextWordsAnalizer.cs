using System.Text.RegularExpressions;

namespace TextAnalizer.Services;

internal class TextWordsAnalizer : ITextAnalizeService
{
    public int GetCount(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        string tempText = Regex.Replace(text, @"\.\.\.", "__ELLIPSIS__");

        string[] words = Regex.Split(tempText, @"\s+");

        int count = 0;
        foreach (string word in words)
        {
            string restoredWord = word.Replace("__ELLIPSIS__", "...");
            if (!string.IsNullOrWhiteSpace(restoredWord) && !Regex.IsMatch(restoredWord, @"^[.,!?;:'""-]+$"))
            {
                count++;
            }
        }

        return count;
    }
}
