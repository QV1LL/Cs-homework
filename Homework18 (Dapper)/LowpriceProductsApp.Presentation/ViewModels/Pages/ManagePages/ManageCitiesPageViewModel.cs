using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;

public partial class ManageCitiesPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    [ObservableProperty]
    public partial Country Country { get; set; } = null!;

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
                Name = value?.Name ?? string.Empty;
                Country = Countries.First(c => c.Id == field?.Country.Id);
            }
        }
    }

    [ObservableProperty]
    public partial bool IsCitySelected {  get; set; }

    public ObservableCollection<City> Cities { get; set; } = new ();
    public List<Country> Countries { get; } = new();

    private readonly ICitiesRepository _cityRepository;

    public ManageCitiesPageViewModel(
        ICitiesRepository cityRepository,
        ICountriesRepository countriesRepository)
    {
        _cityRepository = cityRepository;
        Countries.AddRange(countriesRepository.GetAll());

        UpdateCollection();
    }

    [RelayCommand]
    public void Add(object? parameter)
    {
        try
        {
            var city = new City
            {
                Name = Name,
                Country = Country,
            };

            if (_cityRepository.Find(c => c.Name == city.Name) != null)
                throw new ArgumentException($"City with name {city.Name} is already exist");

            _cityRepository.Add(city);
            UpdateCollection();
        }
        catch(Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public void Update(object? parameter)
    {
        try
        {
            if (SelectedCity == null)
                throw new ArgumentNullException("No one city is selected");

            SelectedCity.Name = Name;
            SelectedCity.Country = Country;
            _cityRepository.Add(SelectedCity);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public void Delete(object? parameter)
    {
        try
        {
            if (SelectedCity == null)
                throw new ArgumentNullException("No one city is selected");

            _cityRepository.Remove(SelectedCity);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    private void UpdateCollection()
    {
        Cities.Clear();
        foreach (var city in _cityRepository.GetAll()) 
            Cities.Add(city);

        IsErrorVisible = false;
    }
}
