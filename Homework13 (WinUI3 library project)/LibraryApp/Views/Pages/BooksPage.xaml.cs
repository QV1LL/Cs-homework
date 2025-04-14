using LibraryApp.ViewModels.PageViewModels;
using Microsoft.UI.Xaml.Controls;

namespace LibraryApp.Views.Pages;

public sealed partial class BooksPage : Page
{
    internal readonly BooksPageViewModel ViewModel;

    public BooksPage()
    {
        InitializeComponent();
        ViewModel = new BooksPageViewModel();
    }
}
