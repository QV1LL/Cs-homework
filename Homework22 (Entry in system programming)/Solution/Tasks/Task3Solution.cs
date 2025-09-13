using System.Diagnostics;

namespace Solution.Tasks;

internal class Task3Solution : ITaskSolution
{
    private const string PROGRAM_NAME = "Calculator.exe";

    public async Task RunAsync()
    {
        try
        {
            IEnumerable<string?> arguments = [Console.ReadLine(), Console.ReadLine(), Console.ReadLine()];

            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = PROGRAM_NAME,
                    Arguments = string.Join(' ', arguments)
                }
            };
            process.Start();
            process.Exited += (s, e) =>
            {
                Console.WriteLine($"Process was working for: {(process.ExitTime - process.StartTime).TotalSeconds}");
                Console.WriteLine($"Process exit code: {process.ExitCode}");
            };

            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex.Message}");
        }
    }
}
