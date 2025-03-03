namespace Task4;

internal static class Program
{
    static void Main(string[] args)
    {
        GetStringParamether getStringParamether = StringOperations.GetCountOfConsonants;

        Console.WriteLine(getStringParamether("DelegatesExampleDemoRepo"));
    }
}
