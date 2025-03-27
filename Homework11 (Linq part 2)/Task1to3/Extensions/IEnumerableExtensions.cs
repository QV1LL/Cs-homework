namespace Task1to3.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<TResult> MySelect<T, TResult>(this IEnumerable<T> values, Func<T, TResult> selector)
    {
        var result = new List<TResult>();

        foreach (var item in values) 
            result.Add(selector(item));

        return result;
    }

    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> values, Predicate<T> predicate)
    {
        var result = new List<T>();

        foreach (var item in values)
            if (predicate(item))
                result.Add(item);

        return result;
    }

}
