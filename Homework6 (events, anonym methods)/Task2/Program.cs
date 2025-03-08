using System.Drawing;
using Task2.Entities;
using Task2.ValueObjects;

namespace Task2;

internal static class Program
{
    static void Main(string[] args)
    {
        TestBag();
    }

    private static void TestBag()
    {
        var bag = new Bag(Color.Black, "Nike", "Leather", maxWeight: 20d);

        bag.ItemAdded += (bag, item) 
            => Console.WriteLine($"{item.Title} added to {bag.Material} bag");
        bag.ItemRemoved += (bag, item) 
            => Console.WriteLine($"{item.Title} removed from {bag.Material} bag");

        PrintBagStats(bag);

        bag.AddItem(new Item("knife", 0.2d, 0.1d));
        bag.AddItem(new Item("rifle", 3d, 1d));
        bag.AddItem(new Item("Grenade", 2d, 2d));

        PrintBagStats(bag);

        bag.AddItem(new Item("???", 5d, 5d));
        bag.RemoveItem(3);

        PrintBagStats(bag);
    }

    private static void PrintBagStats(Bag bag)
    {
        Console.WriteLine($"Weight of bag: {bag.Weight}");
        Console.WriteLine($"Volume of bag: {bag.Volume}");
    }
}
