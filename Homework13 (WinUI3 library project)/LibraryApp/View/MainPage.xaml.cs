using LibraryApp.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LibraryApp.View;

public sealed partial class MainPage : Page
{
    private static MainPageViewModel _viewModel;

    public MainPage()
    {
        this.InitializeComponent();
        
        if (_viewModel == null)
            _viewModel = new MainPageViewModel();

        DataContext = _viewModel;
    }

    private void AddBookButton_Click(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(AddBookPage), new Action<BookViewModel>(_viewModel.AddBook));
    }
}
