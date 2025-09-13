using System.Diagnostics;

namespace Solution.Tasks;

internal class Task1Solution(string ProgramAbsolutePath) : ITaskSolution
{
    public async Task RunAsync()
    {
        try
        {
            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ProgramAbsolutePath
                },
                EnableRaisingEvents = true
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
