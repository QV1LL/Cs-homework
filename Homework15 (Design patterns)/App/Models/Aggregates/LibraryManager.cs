using App.Models.Entities;
using App.Services;

namespace App.Models.Aggregates;

internal class LibraryManager
{
    private static volatile LibraryManager? instance;
    private static readonly object lockObject = new object();

    private readonly List<Book> _books;

    public static LibraryManager Instance
    {
        get
        {
            if (instance == null)
                lock (lockObject)
                    if (instance == null)
                        instance = new LibraryManager();

            return instance;
        }
    }

    private LibraryManager() => _books = new();

    public void AddBook(Book book) => _books.Add(book);

    public Book? PopBook()
    {
        var lastBook = _books.LastOrDefault();
        _books.Remove(lastBook!);
        return lastBook;
    }

    public void PrintBooks()
    {
        foreach (var book in _books)
        {
            PrintBookService.PrintBook(book);
            PrintBookService.PrintEmpty();
        }
    }
}
