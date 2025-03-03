namespace Task4;

public delegate int GetStringParamether(string text);

internal static class StringOperations
{
    public static int GetCountOfVowels(string text) => 
        text?.Count(symbol => "aeuioy".Contains(symbol)) ?? 0;

    public static int GetCountOfConsonants(string text) =>
        text?.Count(symbol => !"aeuioy".Contains(symbol) && char.IsLetter(symbol)) ?? 0;
}
