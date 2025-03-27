using Task4.Extensions;

namespace Task4;

internal class Program
{
    static void Main(string[] args)
    {
        var array = new[] { 78, 81, 121 }.SortByDigitsSum();

        foreach (var item in array)
            Console.WriteLine(item);
    }
}
