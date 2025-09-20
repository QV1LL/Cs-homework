using HorseRaces.Domain;

namespace HorseRaces.Services;

internal interface IRaceService
{
    bool IsRunning { get; }
    IReadOnlyList<Horse> Horses { get; }
    List<Horse> Results { get; }

    void InitializeHorses(int count);
    Task StartRace(Action<Horse> onHorseProgress);
    void ResetRace();
}
