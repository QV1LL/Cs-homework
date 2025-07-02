using GamesApp.Domain.Entities;
using GamesApp.Presentation.Enums;
using GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;

namespace GamesApp.Presentation.Views.Pages.ManagePages;

public sealed partial class ManageGamesPage : Page
{
    public readonly ManageGamesPageViewModel ViewModel;

    private ListViewItemsChangeState _itemsChangeState = ListViewItemsChangeState.None;

    public ManageGamesPage()
    {
        InitializeComponent();
        ViewModel = App.Provider.GetRequiredService<ManageGamesPageViewModel>();
        ViewModel.SelectedGenres.CollectionChanged += SelectedGenres_CollectionChanged;
    }

    private void SelectedGenres_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_itemsChangeState != ListViewItemsChangeState.None)
            return;

        if (GenresListView == null)
            return;

        _itemsChangeState = ListViewItemsChangeState.Changing;

        List<Genre> selectedCities = [.. ViewModel.SelectedGenres];
        GenresListView.SelectedItems.Clear();

        foreach (var city in selectedCities)
            GenresListView.SelectedItems.Add(city);

        _itemsChangeState = ListViewItemsChangeState.None;
    }

    private void GenresListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_itemsChangeState != ListViewItemsChangeState.None)
            return;

        _itemsChangeState = ListViewItemsChangeState.Changing;

        var listView = sender as ListView;
        ViewModel.SelectedGenres.Clear();

        foreach (var item in listView.SelectedItems.Cast<Genre>())
            ViewModel.SelectedGenres.Add(item);

        _itemsChangeState = ListViewItemsChangeState.None;
    }
}
