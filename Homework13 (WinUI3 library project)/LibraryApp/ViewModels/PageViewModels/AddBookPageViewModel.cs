using System;

namespace LibraryApp.ViewModels.PageViewModels;

internal class AddBookPageViewModel : ViewModelBase
{
    public string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Title));

            field = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public string Author
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Author));

            field = value;
            OnPropertyChanged(nameof(Author));
        }
    }

    public string Genre
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Genre));

            field = value;
            OnPropertyChanged(nameof(Genre));
        }
    }

    public string CoverImagePath
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(CoverImagePath));

            field = value;
            OnPropertyChanged(nameof(CoverImagePath));
        }
    }

    public bool IsFavorite
    {
        get => field;
        set
        {
            field = value;
            OnPropertyChanged(nameof(CoverImagePath));
        }
    }
}
