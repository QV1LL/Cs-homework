namespace Task4.Extensions;

internal static class MyExtensions
{
    public static int[] SortByDigitsSum(this int[] values, bool ascending = true)
    {
        var result = values.OrderBy(n => n.GetSumOfDigits());
        return ascending ? result.ToArray() : result.Reverse().ToArray();
    }

    public static int GetSumOfDigits(this int value)
        => value.ToString().ToCharArray().Select(d => (int)char.GetNumericValue(d)).Sum();
}
