namespace DataAudit.Entities;

internal class Product : IAuditable
{
    public Guid Id { get; set; }
    public string Name
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(Name));

            field = value;
        }
    }
    public decimal Price
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(Price));

            field = value;
        }
    }
    public string Description { get; set; }
}
