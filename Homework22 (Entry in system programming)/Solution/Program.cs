using Solution.Tasks;

namespace Solution;

public static class Program
{
    private static readonly List<ITaskSolution> _taskSolutions = [];
    private const string TEST_PROGRAM_PATH = "E:\\Games\\Steins;Gate\\SGrus\\STEINSGATE_RUS.exe";

    static Program()
    {
        _taskSolutions.Add(new Task1Solution(TEST_PROGRAM_PATH));
        _taskSolutions.Add(new Task2Solution(TEST_PROGRAM_PATH));
        _taskSolutions.Add(new Task3Solution());
        _taskSolutions.Add(new Task4Solution());
    }

    static async Task Main(string[] args)
    {
        foreach(var taskSolution in _taskSolutions)
            await taskSolution.RunAsync();
    }
}
