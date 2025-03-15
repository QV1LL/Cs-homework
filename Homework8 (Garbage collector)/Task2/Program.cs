using Task2.Entities;
using SharedKernel.ValueObjects;
using SharedKernel.Enums;

namespace Task2;

internal static class Program
{
    static void Main(string[] args)
    {
        using var shop = new Shop("ATB", new Address("Zahorska", "Ushhorod", "65501"), new PersonName("Andriy", "Pishka"), ShopType.Food);
    }
}
