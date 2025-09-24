using CasinoSimulator.Models;

namespace CasinoSimulator.Services;

internal class PlayersService
{
    public Queue<Player> Players { get; } = [];
    public List<Player> PlayersThatPlayed { get; } = [];
    public List<Player> FinishedPlayers { get; } = [];

    private readonly Random _random = new ();

    public PlayersService()
    {
        var players = Enumerable.Range(1, _random.Next(20, 100))
                      .Select(_ => new Player(_random.Next(200, 500)));

        foreach(var player in players) Players.Enqueue(player);
    }

    public Player GetPlayer()
    {
        return Players.Dequeue();
    }

    public void AddToPlayed(Player player)
    {
        if (PlayersThatPlayed.Contains(player)) return;

        PlayersThatPlayed.Add(player);
    }

    public void FinishPlayer(Player player)
    {
        if (FinishedPlayers.Contains(player)) return;

        player.MakeFinished();
        FinishedPlayers.Add(player);
        PlayersThatPlayed.Remove(player);
    }
}
