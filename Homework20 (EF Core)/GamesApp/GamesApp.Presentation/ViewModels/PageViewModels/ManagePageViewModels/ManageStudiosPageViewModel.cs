using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamesApp.Domain.Entities;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;

public partial class ManageStudiosPageViewModel : PaginationViewModelBase<Studio>
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    public ObservableCollection<City> SelectedCities { get; set; } = new();

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
            {
                Name = field?.Name ?? string.Empty;
                
                SelectedCities.Clear();
                foreach(var city in field!.Cities)
                    SelectedCities.Add(city);
            }
        }
    }

    [ObservableProperty]
    public partial bool IsStudioSelected { get; set; }

    public IReadOnlyCollection<City> Cities => new ReadOnlyCollection<City>([.. _cities]);

    private readonly DbSet<City> _cities;
    private readonly DbSet<Studio> _studios;

    public ManageStudiosPageViewModel(GamesAppContext context) : base(context, context.Studios.Include(s => s.Cities))
    {
        _studios = context.Studios;
        _cities = context.Cities;
    }

    [RelayCommand]
    public async Task Add(object? parameter)
    {
        try
        {
            var studio = new Studio
            {
                Name = Name,
                Cities = [.. SelectedCities]
            };

            if (await _studios
                    .Where(s => s.Name == studio.Name)
                    .FirstOrDefaultAsync() != null)
                throw new ArgumentException($"Studio with name {studio.Name} is already exist");

            _studios.Add(studio);
            await Context.SaveChangesAsync();
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
            SelectedStudio.Cities = [.. SelectedCities];

            _studios.Update(SelectedStudio);
            await Context.SaveChangesAsync();

            var index = Entities.IndexOf(SelectedStudio);
            if (index >= 0 && index < Entities.Count)
                Entities[index!] = SelectedStudio;
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
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }
}
