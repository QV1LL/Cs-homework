namespace Task9.ValueObjects;

internal record class Employee(string name, string surname, int sallary) : IComparable<Employee>
{
    public string Name
    {
        get => name;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Name cannot be empty");

            name = value;
        }
    }

    public string Surname
    {
        get => surname;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Name cannot be empty");

            surname = value;
        }
    }

    public int Sallary
    {
        get => sallary;
        init
        {
            if (sallary < 0)
                throw new ArgumentOutOfRangeException("Sallary must be a non negative value");

            sallary = value;
        }
    }

    public int CompareTo(Employee? other)
    {
        if (other == null) return 1;
        if (this == other) return 0;

        return Sallary.CompareTo(other.Sallary);
    }
}
