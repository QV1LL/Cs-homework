using SharedKernel.Enums;
using SharedKernel.Validation;
using SharedKernel.ValueObjects;

namespace Task2.Entities;

internal class Shop : IDisposable
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Title
    {
        get => field;
        set => field = Validator.GetValidatedValue(value, title => string.IsNullOrEmpty(title), new ArgumentNullException("Title cannot be empty"));
    }

    public Address Address { get; set; }

    public PersonName Owner { get; set; }

    public ShopType Type { get; set; }

    public Shop(string title, Address address, PersonName owner, ShopType type)
    {
        Title = title;
        Address = address;
        Owner = owner;
        Type = type;
    }

    public void Dispose()
    {
        Console.WriteLine("Dispose has been invoked! *_*"); // IDK how to prove the feasibility of using dispose here
    }

    ~Shop()
    {
        Console.WriteLine("Use of destructor -_-"); // same thing...
    }
}
