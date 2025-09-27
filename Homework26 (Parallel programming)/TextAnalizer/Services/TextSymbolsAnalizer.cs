namespace TextAnalizer.Services;

internal class TextSymbolsAnalizer : ITextAnalizeService
{
    public int GetCount(string text) => text.Length;
}
