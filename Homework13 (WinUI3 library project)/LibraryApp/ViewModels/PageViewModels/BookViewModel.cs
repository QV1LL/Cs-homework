using LibraryApp.Domain;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace LibraryApp.ViewModels.PageViewModels;

internal class BookViewModel : ViewModelBase
{
    private readonly Book _book;

    public string Title => _book.Title;
    public string Author => _book.Author;
    public string Genre => _book.Genre;
    public BitmapImage? CoverImage => new (new Uri(_book.CoverImagePath ?? string.Empty));
    public bool IsFavorite => _book.IsFavorite;

    public BookViewModel(Book book) => _book = book;
}
