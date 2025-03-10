namespace Task8.Entities;

internal class Student(string name)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Name must be non empty property");

            name = value;
        }
    }
}
