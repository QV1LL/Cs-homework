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

public partial class ManageStudiosPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public Studio? SelectedStudio
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            IsStudioSelected = field != null;

            if (IsStudioSelected)
                Name = field?.Name ?? string.Empty;
        }
    }

    [ObservableProperty]
    public partial bool IsStudioSelected { get; set; }

    public ObservableCollection<Studio> Studios { get; set; } = new();

    private readonly DbSet<Studio> _studios;
    private readonly DbContext _context;

    public ManageStudiosPageViewModel(GamesAppContext context)
    {
        _context = context;
        _studios = context.Studios;

        foreach (var s in _studios)
            Studios.Add(s);
    }

    [RelayCommand]
    public async Task Add(object? parameter)
    {
        try
        {
            var studio = new Studio
            {
                Name = Name,
            };

            if (await _studios
                    .Where(s => s.Name == studio.Name)
                    .FirstOrDefaultAsync() != null)
                throw new ArgumentException($"Studio with name {studio.Name} is already exist");

            Studios.Add(studio);
            _studios.Add(studio);
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
            if (SelectedStudio == null)
                throw new ArgumentNullException("No one studio is selected");

            SelectedStudio.Name = Name;

            _studios.Update(SelectedStudio);
            await _context.SaveChangesAsync();

            var index = Studios.IndexOf(SelectedStudio);
            if (index >= 0 && index < Studios.Count)
                Studios[index!] = SelectedStudio;
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
            if (SelectedStudio == null)
                throw new ArgumentNullException("No one studio is selected");

            _studios.Remove(SelectedStudio);
            Studios.Remove(SelectedStudio);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }
}
