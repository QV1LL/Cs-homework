namespace App.Models.Entities;

internal class BookBuilder
{
    private string? _title;
    private string? _author;
    private int _year;

    public BookBuilder SetTitle(string title)
    {
        _title = title;
        return this;
    }

    public BookBuilder SetAuthor(string author)
    {
        _author = author;
        return this;
    }
    public BookBuilder SetYear(int year)
    {
        _year = year;
        return this;
    }

    public Book Build() 
        => new Book(_title, _author, _year);
}
