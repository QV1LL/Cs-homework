using LibraryApp.ViewModels.PageViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace LibraryApp.Views;

public sealed partial class AddBookPage : Page
{
    internal readonly AddBookPageViewModel ViewModel;

    public AddBookPage()
    {
        this.InitializeComponent();
        ViewModel = new AddBookPageViewModel();
        ViewModel.ShowDialogRequested += async (s, e) => await MessageDialog.ShowAsync();
    }
}
