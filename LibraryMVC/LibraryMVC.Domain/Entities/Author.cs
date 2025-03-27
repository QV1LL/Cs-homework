using LibraryMVC.Domain.Entities.Interfaces;
using LibraryMVC.Domain.ValueObjects;

namespace LibraryMVC.Domain.Entities;

public class Author : IEntity
{
    public Guid Id { get; set; }
    public Name Name { get; set; }
    private List<Guid> _booksIds;

    public Author()
    {
        if (_booksIds == null)
            _booksIds = new();
    }

    public Author(Name name, List<Guid>? books = null, Guid? id = null)
    {
        Name = name;
        Id = id ?? Guid.NewGuid();
        _booksIds = books ?? new();
    }

    public void AddBook(Guid book) => _booksIds.Add(book);
    public bool IsAuthor(Guid bookId) => _booksIds.Contains(bookId);
    public void RemoveBook(Guid id) => _booksIds.Remove(id);

    public override string ToString() => Name.ToString();
}
