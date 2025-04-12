using LibraryApp.ViewModels.WindowViewModels;
using LibraryApp.Views.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LibraryApp;

public sealed partial class MainWindow : Window
{
    internal MainWindowViewModel ViewModel { get; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new MainWindowViewModel(ContentFrame);
        MainWindowNavigationView.SelectedItem = MainWindowNavigationView.MenuItems[0];
    }

    private void MainWindowNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem item)
        {
            ViewModel.NavigateCommand.Execute(item.Tag?.ToString());
        }
    }
}
