namespace CasinoSimulator.Models;

internal class Player
{
    public Guid Id { get; init; }
    public int Money { get; private set; }
    public int StartMoneyCount { get; }
    public bool IsFinish { get; private set; } = false;
    public bool IsPlayed { get; private set; } = false;

    private int _currentBet;

    public Player(int startMoneyCount)
    {
        Id = Guid.NewGuid();
        Money = startMoneyCount;
        StartMoneyCount = startMoneyCount;
    }

    public void MakeBet(int betCount)
    {
        if (Money < betCount || betCount < 0) return;

        Money -= betCount;
        _currentBet = betCount;
    }

    public void GivePrize()
    {
        Money += 2 * _currentBet;
    }

    public void MakeFinished()
    {
        IsFinish = true;
    }

    public void MakePlayed()
    {
        IsPlayed = true;
    }
}
