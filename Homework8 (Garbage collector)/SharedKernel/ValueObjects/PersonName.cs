using SharedKernel.Validation;

namespace SharedKernel.ValueObjects;

public record PersonName
{
    public static int AlivedObjectsCount { get; private set; } = 0;

    public string FirstName
    {
        get => field;
        init => field = Validator.GetValidatedValue(value, name => string.IsNullOrEmpty(name), new ArgumentNullException("First name cannot be empty"));
    }

    public string MiddleName
    {
        get => field;
        init => field = value ?? "";
    }

    public string LastName
    {
        get => field;
        init => field = Validator.GetValidatedValue(value, name => string.IsNullOrEmpty(name), new ArgumentNullException("Last name cannot be empty"));
    }

    public PersonName(string firstName, string lastName, string middleName = "")
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;

        AlivedObjectsCount++;
    }

    ~PersonName()
    {
        AlivedObjectsCount--;
    }
}
