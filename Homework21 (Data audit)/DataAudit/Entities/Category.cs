namespace DataAudit.Entities;

internal class Category : IAuditable
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
}
