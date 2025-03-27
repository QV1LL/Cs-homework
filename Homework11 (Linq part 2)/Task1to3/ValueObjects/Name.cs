namespace Task1to3.ValueObjects;

public record Name
{
    public string FirstName
    {
        get => field;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException($"{nameof(FirstName)} cannot be null");

            field = value;
        }
    }

    public string LastName
    {
        get => field;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException($"{nameof(LastName)} cannot be null");

            field = value;
        }
    }

    public string? MiddleName { get; init; }


    public Name(string firstName, string lastName, string? middleName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public override int GetHashCode()
        => HashCode.Combine(FirstName, LastName, MiddleName);

    public override string ToString()
        =>  MiddleName == null
            ? $"{FirstName} {LastName}"
            : $"{FirstName} {MiddleName} {LastName}";
}