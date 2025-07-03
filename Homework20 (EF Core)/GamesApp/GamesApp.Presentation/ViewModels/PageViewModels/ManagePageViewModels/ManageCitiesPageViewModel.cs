using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamesApp.Domain.Entities;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;

public partial class ManageCitiesPageViewModel : PaginationViewModelBase<City>
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string Country { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public City? SelectedCity
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            IsCitySelected = field != null;

            if (IsCitySelected)
            {
                Name = field?.Name ?? string.Empty;
                Country = field?.Country ?? string.Empty;
            }
        }
    }

    [ObservableProperty]
    public partial bool IsCitySelected { get; set; }

    private readonly DbSet<City> _cities;

    public ManageCitiesPageViewModel(GamesAppContext context) : base(context, context.Cities)
    {
        _cities = context.Cities;
    }

    [RelayCommand]
    public async Task Add(object? parameter)
    {
        try
        {
            var city = new City
            {
                Name = Name,
                Country = Country,
            };

            if (await EntitiesSet
                    .Where(c => c.Name == city.Name)
                    .FirstOrDefaultAsync() != null)
                throw new ArgumentException($"City with name {city.Name} is already exist");

            _cities.Add(city);
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
            if (SelectedCity == null)
                throw new ArgumentNullException("No one city is selected");

            SelectedCity.Name = Name;
            SelectedCity.Country = Country;

            _cities.Update(SelectedCity);
            await Context.SaveChangesAsync();
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
            if (SelectedCity == null)
                throw new ArgumentNullException("No one city is selected");

            _cities.Remove(SelectedCity);
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }
}
