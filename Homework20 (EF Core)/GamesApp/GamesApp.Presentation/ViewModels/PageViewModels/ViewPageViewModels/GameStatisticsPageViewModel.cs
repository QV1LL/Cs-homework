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
    public int SingleplayerCount => _games.CountAsync(g => g.Type == Domain.Enums.GameType.Singleplayer).Result;
    public int MultiplayerCount => _games.CountAsync(g => g.Type == Domain.Enums.GameType.Multiplayer).Result;

    public Genre? SelectedGenre
    {
        get => field;
        set
        {
            SetProperty(ref field, value);

            if (field != null)
            {
                TopSellingGame = _games.OrderByDescending(g => g.CountOfSales).FirstOrDefault()?.Name ?? "Genre doesnt choosed";
                
                Top5MostSoldGames.Clear();
                foreach(var game in _games.OrderByDescending(g => g.CountOfSales).Take(5))
                    Top5MostSoldGames.Add(game);

                Top5LeastSoldGames.Clear();
                foreach (var game in _games.OrderBy(g => g.CountOfSales).Take(5))
                    Top5LeastSoldGames.Add(game);
            }
        }
    }

    [ObservableProperty]
    public partial string TopSellingGame { get; set; }
    public ObservableCollection<Game> Top5MostSoldGames { get; set; } = new ();
    public ObservableCollection<Game> Top5LeastSoldGames { get; set; } = new();

    public IReadOnlyCollection<Genre> Genres => new ReadOnlyCollection<Genre>([.. _genres]);

    private readonly DbSet<Game> _games;
    private readonly DbSet<Studio> _studios;
    private readonly DbSet<Genre> _genres;
    private readonly DbContext _context;

    public GameStatisticsPageViewModel(GamesAppContext context)
    {
        _context = context;
        _games = context.Games;
        _studios = context.Studios;
        _genres = context.Genres;
    }
}
