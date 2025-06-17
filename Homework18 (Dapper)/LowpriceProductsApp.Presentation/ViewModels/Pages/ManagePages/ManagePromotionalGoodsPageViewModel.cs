using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using LowpriceProductsApp.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;

public partial class ManagePromotionalGoodsPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string StartPrice { get; set; } = string.Empty;

    [ObservableProperty]
    public partial int DiscountPercentage { get; set; }

    [ObservableProperty]
    public partial Country Country { get; set; } = null!;

    [ObservableProperty]
    public partial Section Section { get; set; } = null!;

    [ObservableProperty]
    public partial DateTimeOffset PromotionStart { get; set; } = DateTimeOffset.Now;

    [ObservableProperty]
    public partial DateTimeOffset PromotionEnd { get; set; } = DateTimeOffset.Now;

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; }

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public PromotionalProduct? SelectedProduct
    {
        get => field;
        set
        {
            SetProperty(ref field, value);

            IsProductSelected = field != null;

            if (IsProductSelected)
            {
                Name = field!.Name;
                StartPrice = field!.StartPrice.ToString();
                DiscountPercentage = field!.DiscountPercentage;
                Country = Countries.FirstOrDefault(c => c.Id == field.Country.Id)!;
                Section = Sections.FirstOrDefault(s => s.Id == field.Section.Id)!;
                PromotionStart = field.PromotionStart;
                PromotionEnd = field.PromotionEnd;
            }
        }
    }

    [ObservableProperty]
    public partial bool IsProductSelected { get; set; }

    public ObservableCollection<PromotionalProduct> Products { get; set; } = new();
    public List<Country> Countries { get; } = new();
    public List<Section> Sections { get; } = new();

    private readonly IPromotionalGoodsRepository _productsRepository;
    private readonly ICountriesRepository _countriesRepository;
    private readonly ISectionsRepository _sectionsRepository;

    public ManagePromotionalGoodsPageViewModel(
        IPromotionalGoodsRepository productsRepository,
        ICountriesRepository countriesRepository,
        ISectionsRepository sectionsRepository)
    {
        _productsRepository = productsRepository;
        _countriesRepository = countriesRepository;
        _sectionsRepository = sectionsRepository;

        Countries.AddRange(_countriesRepository.GetAll());
        Sections.AddRange(_sectionsRepository.GetAll());

        UpdateCollection();
    }

    [RelayCommand]
    public void Add()
    {
        try
        {
            var product = new PromotionalProduct
            {
                Name = Name,
                StartPrice = new Money(StartPrice),
                DiscountPercentage = DiscountPercentage,
                Country = Country,
                Section = Section,
                PromotionStart = PromotionStart,
                PromotionEnd = PromotionEnd
            };

            _productsRepository.Add(product);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public void Update()
    {
        try
        {
            if (SelectedProduct == null)
                throw new ArgumentNullException("No promotional product selected");

            var productToUpdate = new PromotionalProduct
            {
                Id = SelectedProduct.Id,
                Name = Name,
                StartPrice = new Money(StartPrice),
                DiscountPercentage = DiscountPercentage,
                Country = Country,
                Section = Section,
                PromotionStart = PromotionStart,
                PromotionEnd = PromotionEnd
            };

            _productsRepository.Add(productToUpdate);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public void Delete()
    {
        try
        {
            if (SelectedProduct == null)
                throw new ArgumentNullException("No promotional product selected");

            _productsRepository.Remove(SelectedProduct);
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
        Products.Clear();
        foreach (var product in _productsRepository.GetAll())
            Products.Add(product);

        IsErrorVisible = false;
    }
}
