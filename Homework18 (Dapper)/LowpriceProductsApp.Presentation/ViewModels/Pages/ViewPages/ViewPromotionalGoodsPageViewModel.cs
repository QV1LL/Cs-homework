using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Infrastructure.Persistence.Repositories.ConcreateRepositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;

public partial class ViewPromotionalGoodsPageViewModel : ObservableObject
{
    private const int PageSize = 10;

    public ObservableCollection<PromotionalProduct> Products { get; set; } = new();
    public Country CountryFilter
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            UpdatePage();
        }
    }
    public Section SectionFilter
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

    private readonly IPromotionalGoodsRepository _promotionalGoodsRepository;
    public List<Country> Countries { get; } = new();
    public List<Section> Sections { get; } = new();
    private PromotionalProduct[] _allProducts = Array.Empty<PromotionalProduct>();

    public ViewPromotionalGoodsPageViewModel(
        IPromotionalGoodsRepository promotionalGoodsRepository,
        ICountriesRepository countriesRepository,
        ISectionsRepository sectionsRepository)
    {
        _promotionalGoodsRepository = promotionalGoodsRepository;
        Countries.AddRange(countriesRepository.GetAll());
        Countries.Add(new Country { Name = "All countries" });
        Sections.AddRange(sectionsRepository.GetAll());
        Sections.Add(new Section { Name = "All sections" });

        LoadData();
    }

    private void LoadData()
    {
        _allProducts = _promotionalGoodsRepository.GetAll().ToArray();

        TotalPages = Math.Max(1, (int)Math.Ceiling(_allProducts.Length / (double)PageSize));
        CurrentPage = 1;

        UpdatePage();
    }

    private void UpdatePage()
    {
        Products.Clear();

        IEnumerable<PromotionalProduct> pageItems = _allProducts;

        if (CountryFilter?.Id != null)
            pageItems = pageItems.Where(c => c.CountryId == CountryFilter.Id);

        if (SectionFilter?.Id != null)
            pageItems = pageItems.Where(c => c.SectionId == SectionFilter.Id);

        pageItems = pageItems
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize);

        foreach (var product in pageItems)
            Products.Add(product);

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
