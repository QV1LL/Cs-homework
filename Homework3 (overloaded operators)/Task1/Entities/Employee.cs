namespace Task1.Entities;

internal class Employee(string fullName, DateTime birthDate)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FullName
    {
        get => field;
        set
        {
            if (value == string.Empty || value == null)
                throw new ArgumentNullException($"{nameof(FullName)} property cannot be null or empty string");

            field = value;
        }
    } = fullName;
    public DateTime BirthDate
    {
        get => field;
        set
        {
            if (DateTime.Now.Year - value.Year < 18)
                throw new ArgumentOutOfRangeException("Employee must be an adult");

            field = value;
        }
    } = birthDate;
}
