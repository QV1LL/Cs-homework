using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.ObjectModel;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;

public partial class ManageSectionsPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public Section? SelectedSection
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            Name = value?.Name ?? string.Empty;
            IsSectionSelected = value != null;
        }
    }

    [ObservableProperty]
    public partial bool IsSectionSelected { get; set; }

    public ObservableCollection<Section> Sections { get; set; } = new();

    private readonly ISectionsRepository _sectionsRepository;

    public ManageSectionsPageViewModel(ISectionsRepository sectionsRepository)
    {
        _sectionsRepository = sectionsRepository;
        UpdateCollection();
    }

    [RelayCommand]
    public void Add(object? parameter)
    {
        try
        {
            var section = new Section { Name = Name };

            if (_sectionsRepository.Find(c => c.Name == section.Name) != null)
                throw new ArgumentException($"City with name {section.Name} is already exist");

            _sectionsRepository.Add(section);
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
            if (SelectedSection == null)
                throw new ArgumentNullException("No one section is selected");

            SelectedSection.Name = Name;
            _sectionsRepository.Add(SelectedSection);
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
            if (SelectedSection == null)
                throw new ArgumentNullException("No one section is selected");

            _sectionsRepository.Remove(SelectedSection);
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
        Sections.Clear();
        foreach (var section in _sectionsRepository.GetAll())
            Sections.Add(section);

        IsErrorVisible = false;
    }
}
