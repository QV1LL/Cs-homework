using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;

public partial class ViewCountriesPageViewModel : ObservableObject
{
    private const int PageSize = 10;

    public ObservableCollection<Country> Countries { get; set; } = new();

    [ObservableProperty]
    public partial int CurrentPage { get; set; } = 1;

    [ObservableProperty]
    public partial int TotalPages { get; set; }

    public bool CanGoPrevious => CurrentPage > 1;
    public bool CanGoNext => CurrentPage < TotalPages;

    public string CurrentPageText => $"Page {CurrentPage} / {TotalPages}";

    private readonly ICountriesRepository _countriesRepository;
    private Country[] _allCountries = Array.Empty<Country>();

    public ViewCountriesPageViewModel(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository;
        LoadData();
    }

    private void LoadData()
    {
        _allCountries = _countriesRepository.GetAll().ToArray();

        TotalPages = Math.Max(1, (int)Math.Ceiling(_allCountries.Length / (double)PageSize));
        CurrentPage = 1;

        UpdatePage();
    }

    private void UpdatePage()
    {
        Countries.Clear();

        var pageItems = _allCountries
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize);

        foreach (var country in pageItems)
            Countries.Add(country);

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

