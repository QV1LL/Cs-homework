namespace Task4;

internal static class Program
{
    public static void Main(string[] args)
    {
        var trash = MemoryAllocator.GenerateTrash(1_000_000_000);

        GC.Collect();

        Console.WriteLine(GC.GetGeneration(trash));
        Console.WriteLine(GC.GetTotalMemory(false) / 1024);

        MemoryAllocator.SimulateMemoryUsage();
    }
}
