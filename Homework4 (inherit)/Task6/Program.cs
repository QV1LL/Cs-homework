using Task6.Entities;
using Task6.ValueObjects;

namespace Task6;

internal class Program
{
    static void Main(string[] args)
    {
        var products = new List<Product>
        {
            new Furniture ("Kitchen", "new kitchen", new Money(39999, 99), FurnitureDiscountFromState.New),
            new Furniture ("Chair", "just chair", new Money(599, 99), FurnitureDiscountFromState.Good),
            new Furniture ("Sofa", "broken sofa", new Money(6000, 00), FurnitureDiscountFromState.Broken),
            new Electronic ("GTX 1650", "Undying classic", new Money(8049, 00), ElectronicDiscountFromActuality.Normal),
            new Electronic ("RTX 5090", "Actual most powerfull graphic cart", new Money(199999, 99), ElectronicDiscountFromActuality.Actual),
            new Electronic ("Radeon HD5450", "...", new Money(599, 99), ElectronicDiscountFromActuality.Deprecated)
        };

        foreach(var product in products)
            Console.WriteLine($"Product: {product.Title}, discount: {product.GetDiscount()}");
    }
}
