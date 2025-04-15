using LibraryApp.Commands;
using LibraryApp.Strategies.Sorting;
using LibraryApp.ViewModels.EntityViewModels;
using LibraryApp.Views;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace LibraryApp.ViewModels.PageViewModels;

internal class BooksPageViewModel : ViewModelBase
{
    private bool _isStackLayout = true;

    public IEnumerable<BookViewModel> FilteredBooksViewModels
    {
        get
        {
            var booksViewModel = App.Books
                .Select(b => new BookViewModel(b))
                .Where(b => (b.Title.Contains(SearchText ?? "") ||
                           b.Author.Contains(SearchText ?? "") ||
                           b.Genre.Contains(SearchText ?? "")) &&
                           (SelectedGenre == "All" || b.Genre == SelectedGenre));

            return SelectedSortingStrategy.Apply(booksViewModel);
        }
    }

    public IEnumerable<string> Genres
    {
        get
        {
            var books = new List<string>() { "All" };
            books.AddRange(App.Books.Select(b => b.Genre).ToHashSet());

            return books;
        }
    }
    public IEnumerable<ISortingStrategy<BookViewModel>> SortingStrategies => 
        new List<ISortingStrategy<BookViewModel>>()
        {
            new AllBooksStrategy(),
            new BooksByGenreStrategy(),
            new FavoriteBooksStrategy()
        };
    public ISortingStrategy<BookViewModel> SelectedSortingStrategy
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanged(nameof(FilteredBooksViewModels));
        }
    } = new AllBooksStrategy();
    public string SelectedGenre
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanged(nameof(FilteredBooksViewModels));
        }
    } = "All";
    public string SearchText
    {
        get => field;
        set
        {
            SetProperty(ref field, value);
            OnPropertyChanged(nameof(FilteredBooksViewModels));
        }
    }
    public Layout CurrentLayout
    {
        get => field;
        set => SetProperty(ref field, value);
    } = new StackLayout();

    public ICommand ToggleLayoutCommand { get; }
    public ICommand AddBookCommand { get; }

    public BooksPageViewModel()
    {
        ToggleLayoutCommand = new RelayCommand(
            (p) =>
            {
                _isStackLayout = !_isStackLayout;
                CurrentLayout = _isStackLayout
                    ? new StackLayout { Orientation = Orientation.Vertical, Spacing = 10 }
                    : new UniformGridLayout
                    {
                        MaximumRowsOrColumns = 3,
                        MinColumnSpacing = 10,
                        MinRowSpacing = 10,
                    };
            }
        );
        AddBookCommand = new RelayCommand(
            (p) => MainWindow.MainWindowContentFrame.Navigate(typeof(AddBookPage))
        );
    }
}
