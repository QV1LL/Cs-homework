using System.Drawing;
using Task2.ValueObjects;

namespace Task2.Entities;

internal delegate void OnBagItemsStateChangedHandler(Bag bag, Item item);

internal class Bag
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public Color Color { get; set; }
    
    public string Brand
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Brand cannot be empty string");

            field = value;
        }
    }
    
    public string Material
    {
        get => field;
        set
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Material cannot be empty string");

            field = value;
        }
    }

    public double MaxWeight
    {
        get => field;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("Weight property cannot be negative or equals to 0");

            field = value;
        }
    }

    public double MaxVolume
    {
        get => field;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException("Volume property cannot be negative or equals to 0");

            field = value;
        }
    }

    public double Weight { get; private set; } = 0d;

    public double Volume { get; private set; } = 0d;

    private List<Item> items = new();

    public event OnBagItemsStateChangedHandler ItemAdded;

    public event OnBagItemsStateChangedHandler ItemRemoved;

    public Bag(
        Color color,
        string brand,
        string material,
        double maxWeight = 10,
        double maxVolume = 30)
    {
        Color = color;
        Brand = brand;
        Material = material;
        MaxWeight = maxWeight;
        MaxVolume = maxVolume;

        ItemAdded += (bag, item) =>
        {
            if (bag.Weight + item.Weight > bag.MaxWeight ||
                bag.Volume + item.Volume > bag.MaxVolume)
                throw new StackOverflowException("Bag is overflow!");

            bag.Weight += item.Weight;
            bag.Volume += item.Volume;
        };

        ItemRemoved += (bag, item) =>
        {
            bag.Weight -= item.Weight;
            bag.Volume -= item.Volume;
        };
    }

    public void AddItem(Item item)
    {
        this.items.Add(item);

        ItemAdded(this, item);
    }

    public void RemoveItem(Index id)
    {
        Item removedItem = this.items.ElementAt(id);

        this.items.RemoveAt(id.Value);

        ItemRemoved(this, removedItem);
    }
}
