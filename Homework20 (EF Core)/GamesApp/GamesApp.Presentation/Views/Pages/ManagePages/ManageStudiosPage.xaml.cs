using GamesApp.Domain.Entities;
using GamesApp.Presentation.Enums;
using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamesApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageStudiosPage : Page
{
    public readonly ManageStudiosPageViewModel ViewModel;

    private ListViewItemsChangeState _itemsChangeState = ListViewItemsChangeState.None;

    public ManageStudiosPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageStudiosPageViewModel>();
        ViewModel.SelectedCities.CollectionChanged += SelectedCities_CollectionChanged;
    }

    private void SelectedCities_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_itemsChangeState != ListViewItemsChangeState.None)
            return;

        if (CitiesListView == null)
            return;

        _itemsChangeState = ListViewItemsChangeState.Changing;

        List<City> selectedCities = [.. ViewModel.SelectedCities];
        CitiesListView.SelectedItems.Clear();

        foreach (var city in selectedCities)
            CitiesListView.SelectedItems.Add(city);

        _itemsChangeState = ListViewItemsChangeState.None;
    }

    private void CitiesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_itemsChangeState != ListViewItemsChangeState.None)
            return;

        _itemsChangeState = ListViewItemsChangeState.Changing;

        var listView = sender as ListView;
        ViewModel.SelectedCities.Clear();

        foreach (var item in listView.SelectedItems.Cast<City>())
            ViewModel.SelectedCities.Add(item);

        _itemsChangeState = ListViewItemsChangeState.None;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        DeleteConfirmationDialog.ShowAsync();
    }
}
