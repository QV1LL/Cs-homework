using Task6.ValueObjects;

namespace Task6.Entities;

internal enum FurnitureDiscountFromState
{
    New = 0,
    Good = 5,
    Beeter = 20,
    Old = 40,
    Broken = 70,
}

internal class Furniture : Product
{
    public FurnitureDiscountFromState DiscountFromState { get; set; }

    public Furniture(string title, string description, Money price, FurnitureDiscountFromState discountFromState)
        : base(title, description, price)
    {
        DiscountFromState = discountFromState;
    }

    public override Money GetDiscount()
        => Price * ((float)DiscountFromState / 100);
}
