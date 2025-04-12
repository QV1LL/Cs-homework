using LibraryApp.Strategies.Sorting;
using LibraryApp.ViewModels.PageViewModels;
using LibraryApp.ViewModels.WindowViewModels;
using Microsoft.UI.Xaml.Controls;
using System;

namespace LibraryApp.Views.Pages;

public sealed partial class BooksPage : Page
{
    public BooksPage(string navigationTag = null)
    {
        try
        {
            MainWindowViewModel mainWindowViewModel;
            if (App.MainWindow is MainWindow mainWindow)
            {
                mainWindowViewModel = mainWindow.ViewModel;
            }
            else
            {
                mainWindowViewModel = new MainWindowViewModel(Frame);
            }

            ISortingStrategy<BookViewModel> sortingStrategy = navigationTag switch
            {
                "AllBooks" => new AllBooksStrategy(),
                "BooksByGenre" => new BooksByGenreStrategy(),
                "FavoriteBooks" => new FavoriteBooksStrategy(),
                _ => new AllBooksStrategy()
            };

            DataContext = new BooksPageViewModel(mainWindowViewModel, sortingStrategy);
            InitializeComponent();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BooksPage initialization failed: {ex.Message}");
            throw;
        }
    }
}
