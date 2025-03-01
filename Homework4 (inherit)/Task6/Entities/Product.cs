using Task6.ValueObjects;

namespace Task6.Entities;

internal abstract class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Title cannot contain empty, null or white space string");

            field = value;
        }
    }
    public string Description
    {
        get => field;
        set
        {
            if (value.IsWhiteSpace() || value == string.Empty || value == null)
                throw new ArgumentNullException("Description cannot contain empty, null or white space string");

            field = value;
        }
    }
    public Money Price { get; set; }

    protected Product(string title, string description, Money price)
    {
        Title = title;
        Description = description;
        Price = price;
    }

    public abstract Money GetDiscount();
}
