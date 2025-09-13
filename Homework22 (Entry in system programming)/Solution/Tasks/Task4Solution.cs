using System.Diagnostics;

namespace Solution.Tasks;

internal class Task4Solution : ITaskSolution
{
    private const string PROGRAM_NAME = "WordsFinder.exe";

    public async Task RunAsync()
    {
        try
        {
            List<string?> arguments = [Console.ReadLine(), Console.ReadLine()];

            if (!File.Exists(arguments[0]))
                throw new ArgumentException($"File with path `{arguments[0]}` does not exists");

            using var stream = File.Open(arguments[0]!, FileMode.Open, FileAccess.Read);
            stream.Close();

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
        catch (UnauthorizedAccessException)
        {
            throw new IOException($"File is not readable (permission denied).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex.Message}");
        }
    }
}
