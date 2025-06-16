using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;

public partial class ManageCountriesPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public Country? SelectedCountry
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            Name = value?.Name ?? string.Empty;
            IsCountrySelected = value != null;
        }
    }

    [ObservableProperty]
    public partial bool IsCountrySelected { get; set; }

    public ObservableCollection<Country> Countries { get; set; } = new();

    private readonly ICountriesRepository _countriesRepository;

    public ManageCountriesPageViewModel(ICountriesRepository cityRepository)
    {
        _countriesRepository = cityRepository;
        UpdateCollection();
    }

    [RelayCommand]
    public void Add(object? parameter)
    {
        try
        {
            var country = new Country { Name = Name };

            if (_countriesRepository.Find(c => c.Name == country.Name) != null)
                throw new ArgumentException($"Country with name {country.Name} is already exist");

            _countriesRepository.Add(country);
            UpdateCollection();
        }
        catch (Exception e)
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
            if (SelectedCountry == null)
                throw new ArgumentNullException("No one country is selected");

            SelectedCountry.Name = Name;
            _countriesRepository.Add(SelectedCountry);
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
            if (SelectedCountry == null)
                throw new ArgumentNullException("No one country is selected");

            _countriesRepository.Remove(SelectedCountry);
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
        Countries.Clear();
        foreach (var country in _countriesRepository.GetAll())
            Countries.Add(country);

        IsErrorVisible = false;
    }
}
