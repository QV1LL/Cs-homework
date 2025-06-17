using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;

public partial class ViewCustomersPageViewModel : ObservableObject
{
    private const int PageSize = 10;

    public ObservableCollection<Customer> Customers { get; set; } = new();

    public Country CountryFilter
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            UpdatePage();
        }
    }
    public City CityFilter
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

    private readonly ICustomersRepository _customersRepository;

    public List<City> Cities { get; } = new();
    public List<Country> Countries { get; } = new();
    private Customer[] _allCustomers = Array.Empty<Customer>();

    public ViewCustomersPageViewModel(
        ICustomersRepository customersRepository,
        ICitiesRepository citiesRepository,
        ICountriesRepository countriesRepository)
    {
        _customersRepository = customersRepository;
        Cities.AddRange(citiesRepository.GetAll());
        Cities.Add(new City { Name = "All cities" });
        Countries.AddRange(countriesRepository.GetAll());
        Countries.Add(new Country { Name = "All countries" });

        LoadData();
    }

    private void LoadData()
    {
        _allCustomers = _customersRepository.GetAll().ToArray();

        TotalPages = Math.Max(1, (int)Math.Ceiling(_allCustomers.Length / (double)PageSize));
        CurrentPage = 1;

        UpdatePage();
    }

    private void UpdatePage()
    {
        Customers.Clear();

        IEnumerable<Customer> pageItems = _allCustomers;

        if (CountryFilter?.Id != null)
            pageItems = pageItems.Where(c => c.City.CountryId == CountryFilter.Id);

        if (CityFilter?.Id != null)
            pageItems = pageItems.Where(c => c.CityId == CityFilter.Id);

        pageItems = pageItems
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize);

        foreach (var customer in pageItems)
            Customers.Add(customer);

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
