using Task1to3.Entities.Enums;
using Task1to3.ValueObjects;

namespace Task1to3.Entities;

internal class EmployeeInfo
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Name Name { get; set; }
    public EmployeePosition Position { get; set; }
    public string PhoneNumber
    {
        get => field;
        set
        {
            if (value == null)
                throw new ArgumentNullException("Phone number cannot be null");

            field = value;
        }
    }
    public string Email
    {
        get => field;
        set
        {
            if (value == null)
                throw new ArgumentNullException("Email cannot be null");

            field = value;
        }
    }
    public Money Salary { get; set; }

    public EmployeeInfo(Name name, EmployeePosition position, string phoneNumber, string email, Money salary)
    {
        Name = name;
        Position = position;
        PhoneNumber = phoneNumber;
        Email = email; 
        Salary = salary;
    }

    public override string ToString()
        => $"Employee: {Name}, Position: {Position}, " +
           $"Phone: {PhoneNumber}, Email: {Email}, Salary: {Salary}";
}
