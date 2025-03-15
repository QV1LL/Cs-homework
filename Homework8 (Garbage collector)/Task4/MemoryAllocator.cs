using System.Diagnostics;

namespace Task4;

internal class MemoryAllocator : IDisposable
{
    private bool _disposed = false;

    public static byte[] GenerateTrash(int countKilobytes) => new byte[countKilobytes * 1024];

    public static void SimulateMemoryUsage()
    {
        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < 100_000; i++)
        {
            var temp = new int[100];
            temp[0] = i;
        }

        stopwatch.Stop();
        Console.WriteLine($"Memory usage simulation completed in {stopwatch.ElapsedMilliseconds} ms.");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); 
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                Console.WriteLine("Managed resources disposed.");
            
            _disposed = true;
        }
    }

    ~MemoryAllocator()
    {
        Dispose(false);
        Console.WriteLine("Finalizer called.");
    }
}
