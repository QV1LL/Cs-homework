using SharedKernel.Enums;
using SharedKernel.Validation;
using SharedKernel.ValueObjects;

namespace Task1.Entities;

internal class Play : IDisposable
{
    public static int AlivedObjectsCount { get; private set; } = 0;

    public Guid Id { get; init; } = Guid.NewGuid();

    public string Title
    {
        get => field;
        set => field = Validator.GetValidatedValue(value, title => string.IsNullOrEmpty(value), new ArgumentNullException("Title cannot be empty"));
    }

    public PersonName AuthorName { get; set; }

    public PlayGenre Genre { get; set; }

    public int RealeseYear
    {
        get => field;
        set => field = Validator.GetValidatedValue(value, year => year > DateTime.Now.Year, new ArgumentOutOfRangeException("Year cannot be above the current"));
    }

    public void Dispose()
    {
        Console.WriteLine("Dispose method was called!"); // use of IDisposable is required in task -_-
    }

    public Play(string title, PersonName authorName, PlayGenre genre, int realeseYear)
    {
        Title = title;
        AuthorName = authorName;
        Genre = genre;
        RealeseYear = realeseYear;

        AlivedObjectsCount++;
    }

    ~Play()
    {
        AlivedObjectsCount--;
    }
}
