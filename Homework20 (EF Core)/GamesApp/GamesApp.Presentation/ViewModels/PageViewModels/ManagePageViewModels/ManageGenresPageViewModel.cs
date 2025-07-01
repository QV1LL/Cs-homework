using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamesApp.Domain.Entities;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;

public partial class ManageGenresPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public Genre? SelectedGenre
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            IsGenreSelected = field != null;

            if (IsGenreSelected)
                Name = field?.Name ?? string.Empty;
        }
    }

    [ObservableProperty]
    public partial bool IsGenreSelected { get; set; }

    public ObservableCollection<Genre> Genres { get; set; } = new();

    private readonly DbSet<Genre> _genres;
    private readonly DbContext _context;

    public ManageGenresPageViewModel(GamesAppContext context)
    {
        _context = context;
        _genres = context.Genres;

        foreach (var g in _genres)
            Genres.Add(g);
    }

    [RelayCommand]
    public async Task Add(object? parameter)
    {
        try
        {
            var genre = new Genre
            {
                Name = Name,
            };

            if (await _genres
                    .Where(g => g.Name == genre.Name)
                    .FirstOrDefaultAsync() != null)
                throw new ArgumentException($"Genre with name {genre.Name} is already exist");

            Genres.Add(genre);
            _genres.Add(genre);
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
            if (SelectedGenre == null)
                throw new ArgumentNullException("No one genre is selected");

            SelectedGenre.Name = Name;

            _genres.Update(SelectedGenre);
            await _context.SaveChangesAsync();

            var index = Genres.IndexOf(SelectedGenre);
            if (index >= 0 && index < Genres.Count)
                Genres[index!] = SelectedGenre;
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
            if (SelectedGenre == null)
                throw new ArgumentNullException("No one genre is selected");

            _genres.Remove(SelectedGenre);
            Genres.Remove(SelectedGenre);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }
}
