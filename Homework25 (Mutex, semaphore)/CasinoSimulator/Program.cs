using CasinoSimulator.Services;

namespace CasinoSimulator;

internal static class Program
{
    static void Main()
    {
        var playersService = new PlayersService();
        var casino = new CasinoService(playersService, countOfTables: 3);
        var logger = new CasinoLoggerService();
        logger.Subscribe(casino, casino.Tables);
        casino.Run();
    }
}
