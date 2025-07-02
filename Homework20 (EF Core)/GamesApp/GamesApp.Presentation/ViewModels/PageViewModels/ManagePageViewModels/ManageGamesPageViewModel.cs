using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamesApp.Domain.Entities;
using GamesApp.Domain.Enums;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;

public partial class ManageGamesPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string GameType { get; set; } = string.Empty;
    [ObservableProperty]
    public partial int CountOfSales { get; set; }
    [ObservableProperty]
    public partial Studio Studio { get; set; } = null!;
    public ObservableCollection<Genre> SelectedGenres { get; set; } = new();

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;
    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public Game? SelectedGame
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            IsGameSelected = field != null;

            if (IsGameSelected)
            {
                Name = field!.Name;
                Description = field!.Description;
                GameType = field!.Type.ToString();
                CountOfSales = field!.CountOfSales;
                Studio = field!.Studio;

                SelectedGenres.Clear();
                foreach(var genre in field!.Genres)
                    SelectedGenres.Add(genre);
            }
        }
    }

    [ObservableProperty]
    public partial bool IsGameSelected { get; set; }

    public IReadOnlyCollection<Genre> Genres => new ReadOnlyCollection<Genre>([.. _genres]);
    public IReadOnlyCollection<Studio> Studios => new ReadOnlyCollection<Studio>([.. _studios]);
    public ObservableCollection<Game> Games { get; set; } = new();

    private readonly DbSet<Game> _games;
    private readonly DbSet<Studio> _studios;
    private readonly DbSet<Genre> _genres;
    private readonly DbContext _context;

    public ManageGamesPageViewModel(GamesAppContext context)
    {
        _context = context;
        _genres = context.Genres;
        _games = context.Games;
        _studios = context.Studios;

        foreach (var g in _games.Include(g => g.Genres))
            Games.Add(g);
    }

    [RelayCommand]
    public async Task Add(object? parameter)
    {
        try
        {
            var game = new Game
            {
                Name = Name,
                Description = Description,
                Type = Enum.Parse<GameType>(GameType),
                CountOfSales = CountOfSales,
                Studio = Studio,
                Genres = [.. SelectedGenres]
            };

            Games.Add(game);
            _games.Add(game);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public async Task Update(object? parameter)
    {
        try
        {
            if (SelectedGame == null)
                throw new ArgumentNullException("No one game is selected");

            SelectedGame.Name = Name;
            SelectedGame.Description = Description;
            SelectedGame.Type = Enum.Parse<GameType>(GameType);
            SelectedGame.CountOfSales = CountOfSales;
            SelectedGame.Studio = Studio;
            SelectedGame.Genres = [.. SelectedGenres];

            _games.Update(SelectedGame);
            await _context.SaveChangesAsync();

            var index = Games.IndexOf(SelectedGame);
            if (index >= 0 && index < Games.Count)
                Games[index!] = SelectedGame;
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public async Task Delete(object? parameter)
    {
        try
        {
            if (SelectedGame == null)
                throw new ArgumentNullException("No one game is selected");

            _games.Remove(SelectedGame);
            Games.Remove(SelectedGame);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }
}
