using System.Text.RegularExpressions;

namespace TextAnalizer.Services;

internal class TextSentencesAnalizer : ITextAnalizeService
{
    public int GetCount(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        string[] abbreviations = { "Mr\\.", "Mrs\\.", "Ms\\.", "Dr\\.", "Prof\\.", "Sr\\.", "Jr\\.", "etc\\.", "vs\\." };

        string tempText = text;
        for (int i = 0; i < abbreviations.Length; i++)
        {
            tempText = Regex.Replace(tempText, $@"\b{abbreviations[i]}\s", $"__ABBREV{i}__");
        }

        tempText = Regex.Replace(tempText, @"\.\.\.", "__ELLIPSIS__");

        string pattern = @"(?<=[.!?])\s+(?=[A-ZА-ЯЁ])|(?<=[.!?])$";
        string[] sentences = Regex.Split(tempText.Trim(), pattern);

        int count = 0;
        foreach (var sentence in sentences.Where(sentence => !string.IsNullOrWhiteSpace(sentence)))
        {
            string restored = sentence;
            
            for (int i = 0; i < abbreviations.Length; i++)
            {
                restored = restored.Replace($"__ABBREV{i}__", abbreviations[i].Replace("\\", ""));
            }

            restored = restored.Replace("__ELLIPSIS__", "...");
            if (!string.IsNullOrWhiteSpace(restored))
            {
                count++;
            }
        }

        return count;
    }
}
