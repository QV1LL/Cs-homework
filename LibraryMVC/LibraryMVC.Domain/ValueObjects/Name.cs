namespace LibraryMVC.Domain.ValueObjects;

public record Name
{
    public string FirstName { get; init; }
    public string? MiddleName { get; init; }
    public string LastName { get; init; }

    public Name(string firstName, string lastName, string? middleName = null)
    {
        FirstName = firstName;
        LastName = lastName;

        MiddleName = middleName ?? string.Empty;
    }

    public override string ToString() => $"{this.LastName} {this.FirstName} {this.MiddleName}";
}
