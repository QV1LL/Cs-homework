using GamesApp.Domain.Entities;
using GamesApp.Domain.Enums;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GamesApp.Infrastructure.Services;

public static class GamesAppContextSeeder
{
    public static async Task SeedAsync(GamesAppContext context)
    {
        if (await context.Games.AnyAsync()) return;

        var genres = new[]
        {
            new Genre { Name = "Visual Novel" },
            new Genre { Name = "Action" },
            new Genre { Name = "Adventure" },
            new Genre { Name = "RPG" },
            new Genre { Name = "Racing" },
            new Genre { Name = "Simulation" },
            new Genre { Name = "Sandbox" },
            new Genre { Name = "Open World" }
        };

        var cities = new[]
        {
            new City { Name = "Tokyo", Country = "Japan" },
            new City { Name = "Los Angeles", Country = "USA" },
            new City { Name = "Warsaw", Country = "Poland" },
            new City { Name = "San Francisco", Country = "USA" },
            new City { Name = "Helsinki", Country = "Finland" },
            new City { Name = "Stockholm", Country = "Sweden" }
        };

        var studios = new[]
        {
            new Studio { Name = "5pb", Cities = [cities[0]] },
            new Studio { Name = "Rockstar Games", Cities = [cities[1], cities[3]] },
            new Studio { Name = "CD Projekt RED", Cities = [cities[2]] },
            new Studio { Name = "Ghost Games", Cities = [cities[1]] },
            new Studio { Name = "Polyphony Digital", Cities = [cities[0]] },
            new Studio { Name = "Mojang", Cities = [cities[5]] },
            new Studio { Name = "Camshaft Software", Cities = [cities[4]] },
            new Studio { Name = "Playground Games", Cities = [cities[5]] }
        };

        var games = new[]
        {
            new Game { Name = "Steins;Gate", CountOfSales = 2000000, Description = "Time-travel visual novel", Type = GameType.Singleplayer, Studio = studios[0], Genres = [genres[0]] },
            new Game { Name = "GTA V", CountOfSales = 180000000, Description = "Open-world action", Type = GameType.Multiplayer, Studio = studios[1], Genres = [genres[1], genres[2], genres[7]] },
            new Game { Name = "The Witcher 3", CountOfSales = 40000000, Description = "Fantasy RPG", Type = GameType.Singleplayer, Studio = studios[2], Genres = [genres[3], genres[2]] },
            new Game { Name = "NFS Heat", CountOfSales = 5000000, Description = "Street racing game", Type = GameType.Singleplayer, Studio = studios[3], Genres = [genres[4]] },
            new Game { Name = "NFS Payback", CountOfSales = 7000000, Description = "Heist and racing", Type = GameType.Singleplayer, Studio = studios[3], Genres = [genres[4]] },
            new Game { Name = "NFS Pro Street", CountOfSales = 3000000, Description = "Pro racing", Type = GameType.Singleplayer, Studio = studios[3], Genres = [genres[4]] },
            new Game { Name = "NFS Underground 2", CountOfSales = 11000000, Description = "Underground racing", Type = GameType.Singleplayer, Studio = studios[3], Genres = [genres[4]] },
            new Game { Name = "NFS Underground", CountOfSales = 8000000, Description = "Street racing classic", Type = GameType.Singleplayer, Studio = studios[3], Genres = [genres[4]] },
            new Game { Name = "GTA IV", CountOfSales = 25000000, Description = "Crime open-world", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[7]] },
            new Game { Name = "RDR 2", CountOfSales = 55000000, Description = "Western adventure", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[2]] },
            new Game { Name = "My Summer Car", CountOfSales = 1000000, Description = "Car building sim", Type = GameType.Singleplayer, Studio = studios[6], Genres = [genres[5], genres[4]] },
            new Game { Name = "Minecraft", CountOfSales = 300000000, Description = "Sandbox creativity", Type = GameType.Multiplayer, Studio = studios[5], Genres = [genres[5], genres[6]] },
            new Game { Name = "Asseto Corsa", CountOfSales = 6000000, Description = "Racing simulator", Type = GameType.Multiplayer, Studio = studios[4], Genres = [genres[4], genres[5]] },
            new Game { Name = "Beamng Drive", CountOfSales = 3000000, Description = "Physics driving sim", Type = GameType.Singleplayer, Studio = studios[6], Genres = [genres[5]] },
            new Game { Name = "Forza Horizon 5", CountOfSales = 12000000, Description = "Open-world racing", Type = GameType.Multiplayer, Studio = studios[7], Genres = [genres[4], genres[7]] }
        };

        await context.Genres.AddRangeAsync(genres);
        await context.Cities.AddRangeAsync(cities);
        await context.Studios.AddRangeAsync(studios);
        await context.Games.AddRangeAsync(games);

        await context.SaveChangesAsync();
    }
}