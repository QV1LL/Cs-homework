namespace Task1;

internal static class Program
{
    public static void Main(string[] args)
    {
        TestDelegate(ArrayOperations<int>.GetSimpleValuesArray);
        TestDelegate(ArrayOperations<int>.GetEvenArray);
        TestDelegate(ArrayOperations<int>.GetFibonacciValuesArray);
        TestDelegate(ArrayOperations<int>.GetOddArray);
    }

    public static void TestDelegate(FilterArray<int> filterArray)
    {
        var numbers = filterArray(Enumerable.Range(1, 1000).ToArray());

        foreach (var number in numbers)
            Console.Write(number + " ");
    }
}
