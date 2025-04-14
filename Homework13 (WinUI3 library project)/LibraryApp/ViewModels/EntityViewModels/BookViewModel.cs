using LibraryApp.Domain;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;

namespace LibraryApp.ViewModels.EntityViewModels;

internal class BookViewModel : ViewModelBase
{
    private readonly Book _book;

    public string Title => _book.Title;
    public string Author => _book.Author;
    public string Genre => _book.Genre;
    public Visibility IsFavourite
        => _book.IsFavorite ? Visibility.Visible : Visibility.Collapsed;
    public BitmapImage? CoverImage => Path.Exists(_book.CoverImagePath) ? 
                                      new (new Uri(_book.CoverImagePath)) : 
                                      null;

    public BookViewModel(Book book) => _book = book;
}
