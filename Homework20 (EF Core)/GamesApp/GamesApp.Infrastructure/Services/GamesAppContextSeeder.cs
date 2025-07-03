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
        await context.Database.EnsureCreatedAsync();

        if (await context.Games.AnyAsync())
            return;

        var genres = new[]
        {
            new Genre { Name = "Visual Novel" },
            new Genre { Name = "Action" },
            new Genre { Name = "Adventure" },
            new Genre { Name = "RPG" },
            new Genre { Name = "Racing" },
            new Genre { Name = "Simulation" },
            new Genre { Name = "Sandbox" },
            new Genre { Name = "Open World" },
            new Genre { Name = "Strategy" },
            new Genre { Name = "Fighting" },
            new Genre { Name = "MMORPG" },
            new Genre { Name = "Horror" },
            new Genre { Name = "Puzzle" },
            new Genre { Name = "Stealth" },
            new Genre { Name = "Shooter" }
        };

        var cities = new[]
        {
            new City { Name = "Tokyo", Country = "Japan" },
            new City { Name = "Los Angeles", Country = "USA" },
            new City { Name = "Warsaw", Country = "Poland" },
            new City { Name = "San Francisco", Country = "USA" },
            new City { Name = "Helsinki", Country = "Finland" },
            new City { Name = "Stockholm", Country = "Sweden" },
            new City { Name = "Seattle", Country = "USA" },
            new City { Name = "London", Country = "UK" },
            new City { Name = "Montreal", Country = "Canada" },
            new City { Name = "Kyoto", Country = "Japan" },
            new City { Name = "Austin", Country = "USA" },
            new City { Name = "Osaka", Country = "Japan" },
            new City { Name = "Vancouver", Country = "Canada" }
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
            new Studio { Name = "Playground Games", Cities = [cities[5]] },
            new Studio { Name = "ZA/UM", Cities = [cities[7]] },
            new Studio { Name = "Blizzard Entertainment", Cities = [cities[1]] },
            new Studio { Name = "Arkane Studios", Cities = [cities[10]] },
            new Studio { Name = "Ubisoft Montreal", Cities = [cities[8]] },
            new Studio { Name = "Nintendo", Cities = [cities[9]] },
            new Studio { Name = "Square Enix", Cities = [cities[0]] },
            new Studio { Name = "Capcom", Cities = [cities[11]] },
            new Studio { Name = "Naughty Dog", Cities = [cities[1]] },
            new Studio { Name = "EA DICE", Cities = [cities[5]] },
            new Studio { Name = "Atlus", Cities = [cities[0]] },
            new Studio { Name = "BioWare", Cities = [cities[8]] },
            new Studio { Name = "Konami", Cities = [cities[0]] },
            new Studio { Name = "Annapurna Interactive", Cities = [cities[1]] },
            new Studio { Name = "Toby Fox", Cities = [cities[1]] },
            new Studio { Name = "Team Cherry", Cities = [cities[1]] },
            new Studio { Name = "ConcernedApe", Cities = [cities[1]] },
            new Studio { Name = "Supergiant Games", Cities = [cities[3]] },
            new Studio { Name = "FromSoftware", Cities = [cities[0]] },
            new Studio { Name = "Sucker Punch Productions", Cities = [cities[6]] },
            new Studio { Name = "Santa Monica Studio", Cities = [cities[1]] },
            new Studio { Name = "Valve", Cities = [cities[6]] },
            new Studio { Name = "Insomniac Games", Cities = [cities[1]] },
            new Studio { Name = "Gearbox Software", Cities = [cities[10]] },
            new Studio { Name = "Bungie", Cities = [cities[6]] }
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
            new Game { Name = "Forza Horizon 5", CountOfSales = 12000000, Description = "Open-world racing", Type = GameType.Multiplayer, Studio = studios[7], Genres = [genres[4], genres[7]] },
            new Game { Name = "Disco Elysium", CountOfSales = 1200000, Description = "Narrative-driven RPG", Type = GameType.Singleplayer, Studio = studios[8], Genres = [genres[3]] },
            new Game { Name = "Warcraft 3", CountOfSales = 1210000, Description = "Real-time strategy", Type = GameType.Multiplayer, Studio = studios[9], Genres = [genres[8]] },
            new Game { Name = "Cyberpunk 2077", CountOfSales = 1210000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[2], Genres = [genres[3], genres[7]] },
            new Game { Name = "Dishonored", CountOfSales = 1230000, Description = "Stealth action", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[13], genres[1]] },
            new Game { Name = "Assassin's Creed 2", CountOfSales = 1230000, Description = "Historical action-adventure", Type = GameType.Singleplayer, Studio = studios[11], Genres = [genres[1], genres[2]] },
            new Game { Name = "Super Smash Bros.", CountOfSales = 1260000, Description = "Fighting game", Type = GameType.Multiplayer, Studio = studios[12], Genres = [genres[9]] },
            new Game { Name = "Final Fantasy XIV", CountOfSales = 1270000, Description = "MMORPG", Type = GameType.Multiplayer, Studio = studios[13], Genres = [genres[10]] },
            new Game { Name = "Resident Evil 2 Remake", CountOfSales = 1270000, Description = "Survival horror", Type = GameType.Singleplayer, Studio = studios[14], Genres = [genres[11]] },
            new Game { Name = "Uncharted 4", CountOfSales = 1280000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[15], Genres = [genres[1], genres[2]] },
            new Game { Name = "Star Wars: Battlefront 2", CountOfSales = 1280000, Description = "Shooter", Type = GameType.Multiplayer, Studio = studios[16], Genres = [genres[14]] },
            new Game { Name = "Tetris", CountOfSales = 1310000, Description = "Puzzle game", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[12]] },
            new Game { Name = "Persona 4", CountOfSales = 1350000, Description = "RPG with social sim", Type = GameType.Singleplayer, Studio = studios[17], Genres = [genres[3]] },
            new Game { Name = "Mass Effect 3", CountOfSales = 1360000, Description = "Sci-fi RPG", Type = GameType.Singleplayer, Studio = studios[18], Genres = [genres[3]] },
            new Game { Name = "Assassin's Creed: Black Flag", CountOfSales = 1360000, Description = "Pirate open-world", Type = GameType.Singleplayer, Studio = studios[11], Genres = [genres[1], genres[7]] },
            new Game { Name = "The Legend of Zelda: Twilight Princess", CountOfSales = 1380000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "Call of Duty: Modern Warfare", CountOfSales = 1380000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[16], Genres = [genres[14]] },
            new Game { Name = "Dragon Age: Origins", CountOfSales = 1390000, Description = "Fantasy RPG", Type = GameType.Singleplayer, Studio = studios[18], Genres = [genres[3]] },
            new Game { Name = "StarCraft", CountOfSales = 1410000, Description = "Real-time strategy", Type = GameType.Multiplayer, Studio = studios[9], Genres = [genres[8]] },
            new Game { Name = "Silent Hill 2", CountOfSales = 1410000, Description = "Survival horror", Type = GameType.Singleplayer, Studio = studios[19], Genres = [genres[11]] },
            new Game { Name = "Monster Hunter: World", CountOfSales = 1430000, Description = "Action RPG", Type = GameType.Multiplayer, Studio = studios[14], Genres = [genres[3], genres[1]] },
            new Game { Name = "Grand Theft Auto: Vice City", CountOfSales = 1490000, Description = "Open-world action", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[7]] },
            new Game { Name = "Outer Wilds", CountOfSales = 1500000, Description = "Exploration adventure", Type = GameType.Singleplayer, Studio = studios[20], Genres = [genres[2]] },
            new Game { Name = "Call of Duty: Black Ops 2", CountOfSales = 1510000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[16], Genres = [genres[14]] },
            new Game { Name = "Fallout 4", CountOfSales = 1520000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[3], genres[7]] },
            new Game { Name = "Metroid Prime", CountOfSales = 1540000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "Undertale", CountOfSales = 1540000, Description = "Indie RPG", Type = GameType.Singleplayer, Studio = studios[21], Genres = [genres[3]] },
            new Game { Name = "Pokémon Emerald/Sapphire/Ruby", CountOfSales = 1540000, Description = "RPG adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[3]] },
            new Game { Name = "Metal Gear Solid 2", CountOfSales = 1580000, Description = "Stealth action", Type = GameType.Singleplayer, Studio = studios[19], Genres = [genres[13]] },
            new Game { Name = "Final Fantasy IX", CountOfSales = 1670000, Description = "Fantasy RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3]] },
            new Game { Name = "Super Smash Bros. Ultimate", CountOfSales = 1700000, Description = "Fighting game", Type = GameType.Multiplayer, Studio = studios[12], Genres = [genres[9]] },
            new Game { Name = "Horizon Zero Dawn", CountOfSales = 1710000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[15], Genres = [genres[3], genres[1]] },
            new Game { Name = "Final Fantasy VI", CountOfSales = 1730000, Description = "Classic RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3]] },
            new Game { Name = "Terraria", CountOfSales = 1770000, Description = "Sandbox adventure", Type = GameType.Multiplayer, Studio = studios[20], Genres = [genres[6], genres[2]] },
            new Game { Name = "Grand Theft Auto 4", CountOfSales = 1780000, Description = "Crime open-world", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[7]] },
            new Game { Name = "Batman: Arkham City", CountOfSales = 1830000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[1], genres[2]] },
            new Game { Name = "Shadow of the Colossus", CountOfSales = 1860000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[15], Genres = [genres[1], genres[2]] },
            new Game { Name = "The Legend of Zelda: Wind Waker", CountOfSales = 1870000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "Final Fantasy Tactics", CountOfSales = 1880000, Description = "Tactical RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3]] },
            new Game { Name = "Super Mario Galaxy", CountOfSales = 1950000, Description = "Platformer", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1]] },
            new Game { Name = "Ghost of Tsushima", CountOfSales = 2020000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[26], Genres = [genres[1], genres[2]] },
            new Game { Name = "God of War: Ragnarok", CountOfSales = 2080000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[27], Genres = [genres[1], genres[2]] },
            new Game { Name = "Super Mario Odyssey", CountOfSales = 2090000, Description = "Platformer", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1]] },
            new Game { Name = "The Elder Scrolls 3: Morrowind", CountOfSales = 2130000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[3], genres[7]] },
            new Game { Name = "Portal 2", CountOfSales = 2150000, Description = "Puzzle platformer", Type = GameType.Singleplayer, Studio = studios[28], Genres = [genres[12]] },
            new Game { Name = "Kingdom Hearts 2", CountOfSales = 2170000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3], genres[1]] },
            new Game { Name = "Hades", CountOfSales = 2220000, Description = "Roguelike action", Type = GameType.Singleplayer, Studio = studios[24], Genres = [genres[1]] },
            new Game { Name = "The Legend of Zelda: Majora's Mask", CountOfSales = 2250000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "Super Smash Bros. Melee", CountOfSales = 2280000, Description = "Fighting game", Type = GameType.Multiplayer, Studio = studios[12], Genres = [genres[9]] },
            new Game { Name = "Half-Life 2", CountOfSales = 2310000, Description = "First-person shooter", Type = GameType.Singleplayer, Studio = studios[28], Genres = [genres[14]] },
            new Game { Name = "Castlevania: Symphony of the Night", CountOfSales = 2330000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[19], Genres = [genres[1], genres[2]] },
            new Game { Name = "Borderlands 2", CountOfSales = 2350000, Description = "Action RPG shooter", Type = GameType.Multiplayer, Studio = studios[30], Genres = [genres[3], genres[14]] },
            new Game { Name = "Goldeneye", CountOfSales = 2370000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[12], Genres = [genres[14]] },
            new Game { Name = "Insomniac's Spider-Man", CountOfSales = 2410000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[29], Genres = [genres[1], genres[2]] },
            new Game { Name = "Grand Theft Auto: San Andreas", CountOfSales = 2430000, Description = "Open-world action", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[7]] },
            new Game { Name = "Pokémon Blue/Red/Yellow", CountOfSales = 2490000, Description = "RPG adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[3]] },
            new Game { Name = "Halo: Reach", CountOfSales = 2510000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[31], Genres = [genres[14]] },
            new Game { Name = "Metal Gear Solid", CountOfSales = 2580000, Description = "Stealth action", Type = GameType.Singleplayer, Studio = studios[19], Genres = [genres[13]] },
            new Game { Name = "Halo 2", CountOfSales = 2610000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[31], Genres = [genres[14]] },
            new Game { Name = "Star Wars: Knights of the Old Republic", CountOfSales = 2620000, Description = "RPG", Type = GameType.Singleplayer, Studio = studios[18], Genres = [genres[3]] },
            new Game { Name = "Super Metroid", CountOfSales = 2630000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "Super Mario Bros. 3", CountOfSales = 2640000, Description = "Platformer", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1]] },
            new Game { Name = "World of Warcraft", CountOfSales = 2640000, Description = "MMORPG", Type = GameType.Multiplayer, Studio = studios[9], Genres = [genres[10]] },
            new Game { Name = "NieR: Automata", CountOfSales = 2650000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3], genres[1]] },
            new Game { Name = "Stardew Valley", CountOfSales = 2650000, Description = "Farming simulation", Type = GameType.Singleplayer, Studio = studios[23], Genres = [genres[5]] },
            new Game { Name = "Pokémon Silver/Gold/Crystal", CountOfSales = 2680000, Description = "RPG adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[3]] },
            new Game { Name = "Red Dead Redemption", CountOfSales = 2680000, Description = "Western action-adventure", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[2]] },
            new Game { Name = "Final Fantasy X", CountOfSales = 2740000, Description = "Fantasy RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3]] },
            new Game { Name = "Call of Duty: Modern Warfare 2", CountOfSales = 2830000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[16], Genres = [genres[14]] },
            new Game { Name = "Sekiro: Shadows Die Twice", CountOfSales = 3080000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[25], Genres = [genres[1], genres[2]] },
            new Game { Name = "Diablo 2", CountOfSales = 3210000, Description = "Action RPG", Type = GameType.Multiplayer, Studio = studios[9], Genres = [genres[3]] },
            new Game { Name = "Grand Theft Auto 5", CountOfSales = 3230000, Description = "Open-world action", Type = GameType.Multiplayer, Studio = studios[1], Genres = [genres[1], genres[7]] },
            new Game { Name = "Metal Gear Solid 3", CountOfSales = 3280000, Description = "Stealth action", Type = GameType.Singleplayer, Studio = studios[19], Genres = [genres[13]] },
            new Game { Name = "Halo", CountOfSales = 3290000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[31], Genres = [genres[14]] },
            new Game { Name = "Super Mario 64", CountOfSales = 3330000, Description = "Platformer", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1]] },
            new Game { Name = "The Elder Scrolls 4: Oblivion", CountOfSales = 3340000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[3], genres[7]] },
            new Game { Name = "Super Mario World", CountOfSales = 3430000, Description = "Platformer", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1]] },
            new Game { Name = "Fallout 3", CountOfSales = 3580000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[3], genres[7]] },
            new Game { Name = "Persona 5", CountOfSales = 3690000, Description = "RPG with social sim", Type = GameType.Singleplayer, Studio = studios[17], Genres = [genres[3]] },
            new Game { Name = "Bioshock", CountOfSales = 3710000, Description = "First-person shooter", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[14]] },
            new Game { Name = "The Legend of Zelda: A Link to the Past", CountOfSales = 3910000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "The Last of Us Part 2", CountOfSales = 4060000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[15], Genres = [genres[1], genres[2]] },
            new Game { Name = "Dark Souls 3", CountOfSales = 4070000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[25], Genres = [genres[3], genres[1]] },
            new Game { Name = "God of War (2018)", CountOfSales = 4590000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[27], Genres = [genres[1], genres[2]] },
            new Game { Name = "Chrono Trigger", CountOfSales = 4730000, Description = "Classic RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3]] },
            new Game { Name = "Hollow Knight", CountOfSales = 5040000, Description = "Metroidvania", Type = GameType.Singleplayer, Studio = studios[22], Genres = [genres[1], genres[2]] },
            new Game { Name = "Halo 3", CountOfSales = 5340000, Description = "First-person shooter", Type = GameType.Multiplayer, Studio = studios[31], Genres = [genres[14]] },
            new Game { Name = "Dark Souls", CountOfSales = 5610000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[25], Genres = [genres[3], genres[1]] },
            new Game { Name = "Final Fantasy VII", CountOfSales = 6050000, Description = "Fantasy RPG", Type = GameType.Singleplayer, Studio = studios[13], Genres = [genres[3]] },
            new Game { Name = "Resident Evil 4", CountOfSales = 6380000, Description = "Survival horror", Type = GameType.Singleplayer, Studio = studios[14], Genres = [genres[11]] },
            new Game { Name = "Fallout: New Vegas", CountOfSales = 6580000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[3], genres[7]] },
            new Game { Name = "Mass Effect 2", CountOfSales = 6690000, Description = "Sci-fi RPG", Type = GameType.Singleplayer, Studio = studios[18], Genres = [genres[3]] },
            new Game { Name = "Bloodborne", CountOfSales = 7120000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[25], Genres = [genres[3], genres[1]] },
            new Game { Name = "The Last of Us", CountOfSales = 8360000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[15], Genres = [genres[1], genres[2]] },
            new Game { Name = "The Witcher 3", CountOfSales = 8660000, Description = "Fantasy RPG", Type = GameType.Singleplayer, Studio = studios[2], Genres = [genres[3], genres[2]] },
            new Game { Name = "The Legend of Zelda: Breath of the Wild", CountOfSales = 9800000, Description = "Open-world adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[2], genres[7]] },
            new Game { Name = "The Legend of Zelda: Ocarina of Time", CountOfSales = 10350000, Description = "Action-adventure", Type = GameType.Singleplayer, Studio = studios[12], Genres = [genres[1], genres[2]] },
            new Game { Name = "Elden Ring", CountOfSales = 10600000, Description = "Action RPG", Type = GameType.Singleplayer, Studio = studios[25], Genres = [genres[3], genres[1]] },
            new Game { Name = "Red Dead Redemption 2", CountOfSales = 12290000, Description = "Western action-adventure", Type = GameType.Singleplayer, Studio = studios[1], Genres = [genres[1], genres[2]] },
            new Game { Name = "The Elder Scrolls 5: Skyrim", CountOfSales = 13950000, Description = "Open-world RPG", Type = GameType.Singleplayer, Studio = studios[10], Genres = [genres[3], genres[7]] }
        };

        await context.Genres.AddRangeAsync(genres);
        await context.Cities.AddRangeAsync(cities);
        await context.Studios.AddRangeAsync(studios);
        await context.Games.AddRangeAsync(games);

        await context.SaveChangesAsync();
    }
}