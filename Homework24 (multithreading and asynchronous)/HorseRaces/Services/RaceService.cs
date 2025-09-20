using HorseRaces.Domain;
using System.Drawing;

namespace HorseRaces.Services;

internal class RaceService : IRaceService
{
    public bool IsRunning { get; private set; }
    public IReadOnlyList<Horse> Horses => _horses;
    public List<Horse> Results { get; private set; } = [];

    private readonly List<Horse> _horses = [];
    private readonly Random _random = new();
    private readonly object _lock = new();

    public void InitializeHorses(int count)
    {
        _horses.Clear();

        for (int i = 1; i <= count; i++)
        {
            var color = Color.FromArgb(_random.Next(256), _random.Next(256), _random.Next(256));
            _horses.Add(new Horse($"Horse {i}", color));
        }
    }

    public async Task StartRace(Action<Horse> onHorseProgress)
    {
        if (IsRunning) return;

        IsRunning = true;

        var tasks = _horses.Select(horse => Task.Run(async () =>
        {
            while (horse.DistanceCompleted < 100)
            {
                float step = (float)_random.NextDouble() * 10;

                lock (_lock)
                {
                    horse.RunStep(step);
                    onHorseProgress(horse);
                }

                await Task.Delay(_random.Next(100, 400));
            }

            Results.Add(horse);
        }));

        await Task.WhenAll(tasks);
        IsRunning = false;
    }

    public void ResetRace()
    {
        if (IsRunning) return;

        Results.Clear();
        foreach (var horse in _horses)
            horse.Reset();
    }
}
