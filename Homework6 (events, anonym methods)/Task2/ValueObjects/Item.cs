namespace Task2.ValueObjects;

internal record class Item(string title, double weight, double volume)
{
    public string Title
    {
        get => title;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Title cannot be empty string");

            title = value;
        }
    }
    
    public double Weight
    {
        get => weight;
        init
        {
            if (weight <= 0)
                throw new ArgumentOutOfRangeException("Weight property cannot be negative or equals to 0");

            weight = value;
        }
    }
    
    public double Volume
    {
        get => volume;
        init
        {
            if (volume <= 0)
                throw new ArgumentOutOfRangeException("Volume property cannot be negative or equals to 0");

            volume = value;
        }
    }
}
