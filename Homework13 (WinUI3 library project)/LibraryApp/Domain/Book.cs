using System;

namespace LibraryApp.Domain;

internal class Book
{
    public string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Title));

            field = value;
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
        }
    }

    public string? CoverImagePath { get; set; }

    public bool IsFavorite { get; set; }

    public Book() { }

    public Book(string title, string author, string genre, string? coverImagePath = null, bool isFavorite = false)
    {
        Title = title;
        Author = author;
        Genre = genre;
        CoverImagePath = coverImagePath;
        IsFavorite = isFavorite;
    }
}
