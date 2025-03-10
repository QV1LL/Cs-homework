namespace Task10.ValueObjects;

internal record class Car(string brand, string model, int realiseYear)
{
    public string Brand
    {
        get => brand;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Brand cannot contains null value");

            brand = value;
        }
    }

    public string Model
    {
        get => model;
        init
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("Model cannot contains null value");

            model = value;
        }
    }

    public int RealiseYear
    {
        get => realiseYear;
        init
        {
            if (value < 1900 || value > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException("Invalid year");

            realiseYear = value;
        }
    }
}
