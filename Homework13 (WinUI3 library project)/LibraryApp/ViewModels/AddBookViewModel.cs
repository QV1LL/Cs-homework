using System;

namespace LibraryApp.ViewModels;

internal class AddBookViewModel : ViewModelBase
{
    private readonly Action<BookViewModel> _addBook;

    public string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException($"{nameof(Title)} cannot be empty");

            field = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public string Description
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException($"{nameof(Description)} cannot be empty");

            field = value;
            OnPropertyChanged(nameof(Description));
        }
    }

    public string AuthorName
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException($"{nameof(AuthorName)} cannot be empty");

            field = value;
            OnPropertyChanged(nameof(AuthorName));
        }
    }

    public AddBookViewModel(Action<BookViewModel> addBook)
    {
        _addBook = addBook;
    }

    public void AddBook()
    {
        _addBook?.Invoke(new BookViewModel(Title, Description, AuthorName));
    }
}
