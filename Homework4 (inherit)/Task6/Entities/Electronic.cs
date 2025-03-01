using Task6.ValueObjects;

namespace Task6.Entities;

internal enum ElectronicDiscountFromActuality
{
    Actual = 0,
    Good = 2,
    Normal = 10,
    Outdated = 30,
    Deprecated = 80,
}

internal class Electronic : Product
{
    public ElectronicDiscountFromActuality DiscountFromActuality { get; set; }

    public Electronic(string title, string description, Money price, ElectronicDiscountFromActuality discountFromActuality)
        : base(title, description, price)
    {
        DiscountFromActuality = discountFromActuality;
    }

    public override Money GetDiscount()
        => Price * ((float)DiscountFromActuality / 100);
}
