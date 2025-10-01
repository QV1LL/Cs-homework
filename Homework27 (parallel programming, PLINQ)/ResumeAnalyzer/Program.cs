using ResumeAnalyzer.Services;

namespace ResumeAnalyzer;

internal static class Program
{
    static void Main(string[] args)
    {
        var reportGenerator = new ReportGenerator(args);
        Console.WriteLine(reportGenerator.GenerateReport());
    }
}
