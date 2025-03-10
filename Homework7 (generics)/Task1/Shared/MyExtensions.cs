namespace Task1To6.Shared;

internal static class MyExtensions
{
    public static bool IsFibonacci(this int number)
    {
        return IsPerfectSquare(5 * number * number + 4) || 
               IsPerfectSquare(5 * number * number - 4);
    }

    public static int GetCountOfWords(this string text) => text.Split(" ").Count();

    public static int GetLengthOfLastWords(this string text) => text.Split(" ")[0].Length;

    public static bool CheckForSubstring(this string text, string substring) => text.Contains(substring);

    public static bool IsValidBrackets(this string input)
    {
        Stack<char> brackets = new Stack<char>();

        Dictionary<char, char> bracketPairs = new Dictionary<char, char>
        {
            { ')', '(' },
            { '}', '{' },
            { ']', '[' }
        };

        foreach (char c in input)
        {
            if (c == '(' || c == '{' || c == '[')
                brackets.Push(c);

            else if (c == ')' || c == '}' || c == ']')
                if (brackets.Count == 0 || brackets.Pop() != bracketPairs[c])
                    return false;
        }

        return brackets.Count == 0;
    }

    public static IEnumerable<T> GetSorted<T>(this IEnumerable<T> values, Predicate<T> predicate)
        => values.OrderBy(new Func<T, bool>(predicate));

    private static bool IsPerfectSquare(long x)
    {
        long sqrt = (long)Math.Sqrt(x);
        return sqrt * sqrt == x;
    }
}
