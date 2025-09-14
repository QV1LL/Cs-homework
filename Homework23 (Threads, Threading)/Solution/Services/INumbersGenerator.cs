namespace Solution.Services;

internal interface INumbersGenerator
{
    event Action<int> NextNumberGenerated;

    void Run();
    void Stop();
    void Pause();
    void Resume();
}
