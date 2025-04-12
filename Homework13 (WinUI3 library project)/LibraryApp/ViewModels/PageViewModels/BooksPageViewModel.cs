using LibraryApp.Commands;
using LibraryApp.Domain;
using LibraryApp.Strategies.Sorting;
using LibraryApp.ViewModels.WindowViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace LibraryApp.ViewModels.PageViewModels;

internal class BooksPageViewModel : ViewModelBase
{
    private readonly ObservableCollection<BookViewModel> _allBooks;
    private ObservableCollection<BookViewModel> _filteredBooks;
    private string _selectedGenre;
    private string _searchText;
    private Layout _currentLayout;
    private string _layoutToggleButtonText;
    private ISortingStrategy<BookViewModel> _sortingStrategy;

    public ObservableCollection<BookViewModel> FilteredBooks
    {
        get => _filteredBooks;
        set => SetProperty(ref _filteredBooks, value);
    }

    public List<string> Genres { get; }

    public string SelectedGenre
    {
        get => _selectedGenre;
        set
        {
            if (SetProperty(ref _selectedGenre, value))
            {
                UpdateFilteredBooks();
            }
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                UpdateFilteredBooks();
            }
        }
    }

    public Layout CurrentLayout
    {
        get => _currentLayout;
        set => SetProperty(ref _currentLayout, value);
    }

    public string LayoutToggleButtonText
    {
        get => _layoutToggleButtonText;
        set => SetProperty(ref _layoutToggleButtonText, value);
    }

    public ICommand ToggleLayoutCommand { get; }

    public BooksPageViewModel(MainWindowViewModel mainWindowViewModel, ISortingStrategy<BookViewModel> sortingStrategy)
    {
        try
        {
            _allBooks = new ObservableCollection<BookViewModel>(LoadBooks().Select(b => new BookViewModel(b)));
            _filteredBooks = new ObservableCollection<BookViewModel>(_allBooks);
            Genres = new List<string> { "All Genres" }.Concat(_allBooks.Select(b => b.Genre).Distinct().OrderBy(g => g)).ToList();
            _selectedGenre = "All Genres";
            _searchText = string.Empty;
            _sortingStrategy = sortingStrategy ?? new AllBooksStrategy(); // Default strategy
            _currentLayout = new UniformGridLayout
            {
                MaximumRowsOrColumns = 3,
                MinItemWidth = 200,
                MinItemHeight = 300
            };
            _layoutToggleButtonText = "Switch to Stack Layout";
            ToggleLayoutCommand = new ToggleLayoutCommand(
                layout => CurrentLayout = layout,
                text => LayoutToggleButtonText = text,
                _currentLayout);

            UpdateFilteredBooks();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"BooksPageViewModel initialization failed: {ex.Message}");
            throw;
        }
    }

    private void OnNavigationChanged(object sender, string tag)
    {
        _sortingStrategy = tag switch
        {
            "AllBooks" => new AllBooksStrategy(),
            "BooksByGenre" => new BooksByGenreStrategy(),
            "FavoriteBooks" => new FavoriteBooksStrategy(),
            _ => new AllBooksStrategy()
        };
        UpdateFilteredBooks();
    }

    private void UpdateFilteredBooks()
    {
        try
        {
            var filtered = _allBooks.AsEnumerable();

            // Apply strategy
            filtered = _sortingStrategy.Apply(filtered);

            // Apply genre filter
            if (_selectedGenre != "All Genres")
            {
                filtered = filtered.Where(b => b.Genre == _selectedGenre);
            }

            // Apply search filter
            if (!string.IsNullOrEmpty(_searchText))
            {
                string searchTextLower = _searchText.ToLower();
                filtered = filtered.Where(b => b.Title.ToLower().Contains(searchTextLower) || b.Author.ToLower().Contains(searchTextLower));
            }

            FilteredBooks = new ObservableCollection<BookViewModel>(filtered);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateFilteredBooks failed: {ex.Message}");
        }
    }

    private static IEnumerable<Book> LoadBooks()
    {
        try
        {
            return new List<Book>
            {
                new Book { Title = "Book 1", Author = "Author 1", Genre = "Fiction", CoverImagePath = "ms-appx:///Assets/book1.jpg", IsFavorite = true },
                new Book { Title = "Book 2", Author = "Author 2", Genre = "Non-Fiction", CoverImagePath = "ms-appx:///Assets/book2.jpg" },
                new Book { Title = "Book 3", Author = "Author 3", Genre = "Fiction", CoverImagePath = "ms-appx:///Assets/book3.jpg" }
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"LoadBooks failed: {ex.Message}");
            return new List<Book>();
        }
    }
}
