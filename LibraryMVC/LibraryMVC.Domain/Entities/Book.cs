using LibraryMVC.Domain.Entities.Interfaces;

namespace LibraryMVC.Domain.Entities;

public class Book : IEntity
{
    public Guid Id { get; set; }
    public string Title
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException($"{nameof(value)} is empty");

            field = value;
        }
    }
    public Guid AuthorId { get; set; }

    public Book()
    {

    }

    public Book(string title, Guid authorId, Guid? id = null)
    {
        Id = id ?? Guid.NewGuid();
        Title = title;
        AuthorId = authorId;
    }
}
