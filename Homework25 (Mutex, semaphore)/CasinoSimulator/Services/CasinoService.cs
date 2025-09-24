using CasinoSimulator.Models;

namespace CasinoSimulator.Services;

internal class CasinoService
{
    public readonly List<CasinoTableService> Tables = [];

    private readonly PlayersService _playersService;
    private readonly int _countOfTables;

    public event Action<Player, int, int> PlayerFinished;

    public CasinoService(PlayersService playersService, int countOfTables = 3)
    {
        _playersService = playersService;
        _countOfTables = countOfTables;

        for (int i = 0; i < _countOfTables; i++)
        {
            var table = new CasinoTableService(_playersService);
            table.PlayerFinished += (player, start, end) => PlayerFinished?.Invoke(player, start, end);
            Tables.Add(table);
        }
    }

    public void Run()
    {
        foreach (var table in Tables) table.Run();

        while (_playersService.Players.Any() || _playersService.PlayersThatPlayed.Any())
            Thread.Sleep(500);

        SaveReport();
    }

    private void SaveReport()
    {
        var lines = _playersService.FinishedPlayers
            .Select(p => $"Гравець {p.Id}, [{p.StartMoneyCount}] -> [{p.Money}]")
            .ToArray();
        File.WriteAllLines("casino_report.txt", lines);
    }
}
