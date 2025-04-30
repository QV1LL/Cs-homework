using App.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace App.Models.Entities;

internal class Book : ICloneable
{
    public Guid Id { get; init; } = Guid.NewGuid();
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Author is required")]
    public string Author { get; set; }
    [Year]
    public int Year { get; set; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public object Clone()
        => new Book(Title, Author, Year);
}
