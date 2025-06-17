using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ViewPages;

public partial class ViewSectionsPageViewModel : ObservableObject
{
    private const int PageSize = 10;

    public ObservableCollection<Section> Sections { get; set; } = new();

    [ObservableProperty]
    public partial int CurrentPage { get; set; } = 1;

    [ObservableProperty]
    public partial int TotalPages { get; set; }

    public bool CanGoPrevious => CurrentPage > 1;
    public bool CanGoNext => CurrentPage < TotalPages;

    public string CurrentPageText => $"Page {CurrentPage} / {TotalPages}";

    private readonly ISectionsRepository _sectionsRepository;
    private Section[] _allSections = Array.Empty<Section>();

    public ViewSectionsPageViewModel(ISectionsRepository sectionsRepository)
    {
        _sectionsRepository = sectionsRepository;
        LoadData();
    }

    private void LoadData()
    {
        _allSections = _sectionsRepository.GetAll().ToArray();

        TotalPages = Math.Max(1, (int)Math.Ceiling(_allSections.Length / (double)PageSize));
        CurrentPage = 1;

        UpdatePage();
    }

    private void UpdatePage()
    {
        Sections.Clear();

        var pageItems = _allSections
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize);

        foreach (var section in pageItems)
            Sections.Add(section);

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
