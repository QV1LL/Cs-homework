using Solution.Enums;
using System.Diagnostics;

namespace Solution.Tasks;

internal class Task2Solution(string ProgramAbsolutePath) : ITaskSolution
{
    public async Task RunAsync()
    {
        try
        {
            using var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ProgramAbsolutePath,
                }
            };
            process.Start();
            process.Exited += (s, e) =>
            {
                Console.WriteLine($"Process was working for: {(process.ExitTime - process.StartTime).TotalSeconds}");
                Console.WriteLine($"Process exit code: {process.ExitCode}");
            };

            var userChoice = GetUserChoice();

            if (userChoice == ProcessUserChoice.Close)
                process.Kill();

            await process.WaitForExitAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex.Message}");
        }
    }

    private static ProcessUserChoice GetUserChoice()
    {
        while(true)
        {
            Console.WriteLine("Close program or wait until the end? (close, wait)");

            var input = Console.ReadLine();
            var success = Enum.TryParse(input?.Trim(), out ProcessUserChoice choice);

            if (success) return choice;

            Console.WriteLine($"Failed to parse choice: {input}");
        }
    }
}
