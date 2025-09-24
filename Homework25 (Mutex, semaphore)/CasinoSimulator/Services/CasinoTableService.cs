using CasinoSimulator.Models;

namespace CasinoSimulator.Services;

internal class CasinoTableService
{
    private readonly int _maxPlayersAtTable;
    private readonly PlayersService _playersService;
    private readonly Random _random = new();
    private readonly SemaphoreSlim _seatsSemaphore;

    public event Action<Player, int, int> PlayerFinished;
    public event Action<Player, int, int, int, bool> PlayerMove;

    public CasinoTableService(PlayersService playersService, int maxPlayersAtTable = 5)
    {
        _playersService = playersService;
        _maxPlayersAtTable = maxPlayersAtTable;
        _seatsSemaphore = new SemaphoreSlim(_maxPlayersAtTable, _maxPlayersAtTable);
    }

    public void Run()
    {
        Task.Run(async () =>
        {
            while (_playersService.Players.Any())
            {
                await _seatsSemaphore.WaitAsync();
                if (!_playersService.Players.Any())
                {
                    _seatsSemaphore.Release();
                    break;
                }

                var player = _playersService.GetPlayer();
                await Task.Run(async () =>
                {
                    int startMoney = player.Money;
                    await PlayRounds(player);
                    _playersService.FinishPlayer(player);
                    PlayerFinished?.Invoke(player, startMoney, player.Money);
                    _seatsSemaphore.Release();
                });
            }
        });
    }

    private async Task PlayRounds(Player player)
    {
        int rounds = _random.Next(1, 5);
        for (int i = 0; i < rounds && player.Money > 0; i++)
        {
            int bet = _random.Next(1, player.Money + 1);
            int chosenNumber = _random.Next(0, 37);
            int rouletteNumber = _random.Next(0, 37);

            player.MakeBet(bet);
            player.MakePlayed();

            bool win = chosenNumber == rouletteNumber;
            if (win)
                player.GivePrize();

            PlayerMove?.Invoke(player, bet, chosenNumber, rouletteNumber, win);
            await Task.Delay(_random.Next(100, 500));
        }
    }
}
