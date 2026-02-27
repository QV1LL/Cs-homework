using DependencyInjectionSample.Services.Contracts;

namespace DependencyInjectionSample.Services.Implementation;

public class FileLogger(string filePath) : ILogger, IDisposable
{
    private readonly StreamWriter _writer = new(filePath, append: true);

    public void Log(string message)
    {
        _writer.WriteLine($"{DateTime.Now}: {message}");
        _writer.Flush(); 
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}