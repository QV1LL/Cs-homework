using CommunityToolkit.Mvvm.ComponentModel;
using GamesApp.Domain.Entities;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GamesApp.Presentation.ViewModels.PageViewModels.ViewPageViewModels;

public partial class GameStatisticsPageViewModel : ObservableObject
{
    public string SingleplayerCountInformation => $"Singleplayer: {_games.CountAsync(g => g.Type == Domain.Enums.GameType.Singleplayer).Result}";
    public string MultiplayerCountInformation => $"Multiplayer: {_games.CountAsync(g => g.Type == Domain.Enums.GameType.Multiplayer).Result}";

    public Genre? SelectedGenre
    {
        get => field;
        set
        {
            SetProperty(ref field, value);

            if (field != null) SetupInformation(field);
        }
    }

    [ObservableProperty]
    public partial Game? SelectedGame { get; set; }

    [ObservableProperty]
    public partial string TopSellingGame { get; set; }

    public ObservableCollection<Game> Top5MostSoldGames { get; set; } = new ();
    public ObservableCollection<Game> Top5LeastSoldGames { get; set; } = new();

    public ObservableCollection<Studio> Top3StudiosByGames { get; set; } = new();
    public string MostProductiveStudioText { get; private set; } = string.Empty;

    public ObservableCollection<Genre> Top3Genres { get; set; } = new();
    public string MostPopularGenreText { get; private set; } = string.Empty;

    public ObservableCollection<Genre> Top3GenresBySales { get; set; } = new();
    public string MostPopularGenreBySales { get; private set; } = string.Empty;

    public ObservableCollection<Game> Top3SingleplayerGames { get; set; } = new();
    public string MostPopularSingleplayerGame { get; private set; } = string.Empty;

    public ObservableCollection<Game> Top3MultiplayerGames { get; set; } = new();
    public string MostPopularMultiplayerGame { get; private set; } = string.Empty;

    public IReadOnlyCollection<Genre> Genres => 
        new ReadOnlyCollection<Genre>([.. _genres]);
    public IReadOnlyCollection<Game> Games => 
        new ReadOnlyCollection<Game>([.. _games.Include(g => g.Studio).Include(g => g.Genres)]);
    public IReadOnlyCollection<Studio> Studios => 
        new ReadOnlyCollection<Studio>([.. _studios.Include(s => s.Games)]);

    private readonly DbSet<Game> _games;
    private readonly DbSet<Studio> _studios;
    private readonly DbSet<Genre> _genres;

    public GameStatisticsPageViewModel(GamesAppContext context)
    {
        _games = context.Games;
        _studios = context.Studios;
        _genres = context.Genres;

        InitializeStudioGenreStats();
    }

    private void SetupInformation(Genre genre)
    {
        var gamesByGenre = _games.Where(g => g.Genres.Contains(genre));

        if (!gamesByGenre.Any()) return;

        TopSellingGame = gamesByGenre.OrderByDescending(g => g.CountOfSales).FirstOrDefault()?.Name ?? "Genre doesnt choosed";

        Top5MostSoldGames.Clear();
        foreach (var game in gamesByGenre.OrderByDescending(g => g.CountOfSales).Take(5))
            Top5MostSoldGames.Add(game);

        Top5LeastSoldGames.Clear();
        foreach (var game in gamesByGenre.OrderBy(g => g.CountOfSales).Take(5))
            Top5LeastSoldGames.Add(game);
    }

    private void InitializeStudioGenreStats()
    {
        InitTopStudios();
        InitTopGenresByCount();
        InitTopGenresBySales();
        InitTopGamesByMode();
    }


    private void InitTopStudios()
    {
        var studios = _studios
            .Include(s => s.Games)
            .OrderByDescending(s => s.Games.Count)
            .Take(3)
            .ToList();

        Top3StudiosByGames.Clear();
        foreach (var studio in studios)
            Top3StudiosByGames.Add(studio);

        var topStudio = studios.FirstOrDefault();
        MostProductiveStudioText = topStudio != null
            ? $"{topStudio.Name} — {topStudio.Games.Count} games"
            : "No studios available";
    }

    private void InitTopGenresByCount()
    {
        var genreGroups = _games
            .Include(g => g.Genres)
            .SelectMany(g => g.Genres)
            .GroupBy(g => g.Id)
            .Select(g => new
            {
                Genre = g.First(),
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .ToList();

        Top3Genres.Clear();
        foreach (var genre in genreGroups.Take(3).Select(g => g.Genre))
            Top3Genres.Add(genre);

        var topGenre = genreGroups.FirstOrDefault();
        MostPopularGenreText = topGenre != null
            ? $"{topGenre.Genre.Name} — {topGenre.Count} games"
            : "No genres available";
    }

    private void InitTopGenresBySales()
    {
        var genreSales = _games
            .Include(g => g.Genres)
            .SelectMany(g => g.Genres, (game, genre) => new { genre, game.CountOfSales })
            .GroupBy(x => x.genre.Id)
            .Select(g => new
            {
                Genre = g.First().genre,
                Sales = g.Sum(x => x.CountOfSales)
            })
            .OrderByDescending(g => g.Sales)
            .ToList();

        Top3GenresBySales.Clear();
        foreach (var genre in genreSales.Take(3).Select(g => g.Genre))
            Top3GenresBySales.Add(genre);

        var topGenre = genreSales.FirstOrDefault();
        MostPopularGenreBySales = topGenre != null
            ? $"{topGenre.Genre.Name} — {topGenre.Sales} sales"
            : "No genres available";
    }

    private void InitTopGamesByMode()
    {
        var allGames = _games.Include(g => g.Genres).Include(g => g.Studio).ToList();

        Top3SingleplayerGames.Clear();
        foreach (var game in allGames
                     .Where(g => g.Type == Domain.Enums.GameType.Singleplayer)
                     .OrderByDescending(g => g.CountOfSales)
                     .Take(3))
        {
            Top3SingleplayerGames.Add(game);
        }

        Top3MultiplayerGames.Clear();
        foreach (var game in allGames
                     .Where(g => g.Type == Domain.Enums.GameType.Multiplayer)
                     .OrderByDescending(g => g.CountOfSales)
                     .Take(3))
        {
            Top3MultiplayerGames.Add(game);
        }

        MostPopularSingleplayerGame = Top3SingleplayerGames.FirstOrDefault()?.Name ?? "No data";
        MostPopularMultiplayerGame = Top3MultiplayerGames.FirstOrDefault()?.Name ?? "No data";
    }

}
