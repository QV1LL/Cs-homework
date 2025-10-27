using System;
using System.Text;

namespace SpyMailer;

internal static class CryptoHelper
{
    private const string EnglishAlphabetLower = "abcdefghijklmnopqrstuvwxyz";
    private const string EnglishAlphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string UkrainianAlphabetLower = "абвгґдежзийіїклмнопрстуфхцчшщьюя";
    private const string UkrainianAlphabetUpper = "АБВГҐДЕЖЗИЙІЇКЛМНОПРСТУФХЦЧШЩЬЮЯ";

    public static string Encrypt(string text, int shift)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        return Transform(text, shift);
    }

    public static string Decrypt(string text, int shift)
    {
        if (text == null)
            throw new ArgumentNullException(nameof(text));

        return Transform(text, -shift);
    }

    private static string Transform(string input, int shift)
    {
        var sb = new StringBuilder(input.Length);

        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                if (EnglishAlphabetLower.Contains(c))
                    sb.Append(ShiftChar(c, shift, EnglishAlphabetLower));
                else if (EnglishAlphabetUpper.Contains(c))
                    sb.Append(ShiftChar(c, shift, EnglishAlphabetUpper));
                else if (UkrainianAlphabetLower.Contains(c))
                    sb.Append(ShiftChar(c, shift, UkrainianAlphabetLower));
                else if (UkrainianAlphabetUpper.Contains(c))
                    sb.Append(ShiftChar(c, shift, UkrainianAlphabetUpper));
                else
                    sb.Append(c);
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    private static char ShiftChar(char c, int shift, string alphabet)
    {
        int length = alphabet.Length;
        int index = alphabet.IndexOf(c);
        if (index == -1)
            return c;

        int newIndex = (index + shift) % length;
        if (newIndex < 0)
            newIndex += length;

        return alphabet[newIndex];
    }
}
