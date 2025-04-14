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
    private bool _isStackLayout = false;
    private ISortingStrategy<BookViewModel> _sortingStrategy;

    public IEnumerable<BookViewModel> FilteredBooksViewModels 
        => _sortingStrategy.Apply(App.Books.Select(b => new BookViewModel(b)));
    public IEnumerable<string> Genres => App.Books.Select(b => b.Genre);
    public string SelectedGenre
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public string SearchText
    {
        get => field;
        set => SetProperty(ref field, value);
    }
    public Layout CurrentLayout
    {
        get => field;
        set => SetProperty(ref field, value);
    } = App.CurrentBooksViewLayout;

    public ICommand ToggleLayoutCommand { get; }
    public ICommand AddBookCommand { get; }

    public BooksPageViewModel()
    {
        _sortingStrategy = new AllBooksStrategy();
        ToggleLayoutCommand = new RelayCommand(
            (p) =>
            {
                _isStackLayout = !_isStackLayout;
                App.CurrentBooksViewLayout = _isStackLayout
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
