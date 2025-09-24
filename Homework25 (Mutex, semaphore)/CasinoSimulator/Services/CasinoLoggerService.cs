using CasinoSimulator.Models;

namespace CasinoSimulator.Services;

internal class CasinoLoggerService
{
    public void Subscribe(CasinoService casino, IEnumerable<CasinoTableService> tables)
    {
        casino.PlayerFinished += OnPlayerFinished;
        foreach (var table in tables)
        {
            table.PlayerMove += OnPlayerMove;
        }
    }

    private void OnPlayerFinished(Player player, int startMoney, int endMoney)
    {
        Console.WriteLine($"Гравець {player.Id} [{startMoney}] -> [{endMoney}]");
    }

    private void OnPlayerMove(Player player, int bet, int chosenNumber, int rouletteNumber, bool win)
    {
        string result = win ? "виграв" : "програв";
        Console.WriteLine($"Гравець {player.Id} ставить {bet} на {chosenNumber}, рулетка {rouletteNumber} -> {result}");
    }
}
