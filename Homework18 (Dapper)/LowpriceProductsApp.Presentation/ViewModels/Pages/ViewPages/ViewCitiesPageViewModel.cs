using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;

public partial class ViewCitiesPageViewModel : ObservableObject
{
    private const int PageSize = 10;

    public ObservableCollection<City> Cities { get; set; } = new();

    public Country CountryFilter
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            UpdatePage();
        }
    }

    [ObservableProperty]
    public partial int CurrentPage { get; set; } = 1;

    [ObservableProperty]
    public partial int TotalPages { get; set; }

    public bool CanGoPrevious => CurrentPage > 1;
    public bool CanGoNext => CurrentPage < TotalPages;

    public string CurrentPageText => $"Page {CurrentPage} / {TotalPages}";

    public List<Country> Countries { get; } = new();

    private readonly ICitiesRepository _citiesRepository;
    private City[] _allCities = Array.Empty<City>();

    public ViewCitiesPageViewModel(
        ICitiesRepository citiesRepository,
        ICountriesRepository countriesRepository)
    {
        _citiesRepository = citiesRepository;
        Countries.AddRange(countriesRepository.GetAll());
        Countries.Add(new Country { Name = "All countries"});

        LoadData();
    }

    private void LoadData()
    {
        _allCities = _citiesRepository.GetAll().ToArray();

        TotalPages = Math.Max(1, (int)Math.Ceiling(_allCities.Length / (double)PageSize));
        CurrentPage = 1;

        UpdatePage();
    }

    private void UpdatePage()
    {
        Cities.Clear();

        IEnumerable<City> pageItems = _allCities;

        if (CountryFilter?.Id != null)
            pageItems = pageItems.Where(c => c.CountryId == CountryFilter.Id);

        pageItems = pageItems
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize);

        foreach (var city in pageItems)
            Cities.Add(city);

        OnPropertyChanged(nameof(CanGoPrevious));
        OnPropertyChanged(nameof(CanGoNext));
        OnPropertyChanged(nameof(CurrentPageText));
    }

    [RelayCommand]
    private void NextPage()
    {
        if (CanGoNext)
        {
            CurrentPage++;
            UpdatePage();
        }
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (CanGoPrevious)
        {
            CurrentPage--;
            UpdatePage();
        }
    }
}
