using System.Globalization;

namespace Task1.ValueObjects;

internal record class Money(int wholePart, int decimalPart)
{
    public int WholePart
    {
        get => field;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Whole part must be non negative");

            field = value;
        }
    } = wholePart;
    public int DecimalPart 
    {
        get => field;
        init
        {
            if (value < 0 || value > 99)
                throw new ArgumentOutOfRangeException("Decimal part must be in range from 0 to 99");

            field = value;
        }
    } = decimalPart;

    public override string ToString()
    {
        var regionInfo = new RegionInfo(CultureInfo.CurrentCulture.Name);
        var numberFormat = CultureInfo.CurrentCulture.NumberFormat;

        return $"{this.WholePart}{numberFormat.NumberDecimalSeparator}{this.DecimalPart}{regionInfo.CurrencySymbol}";
    }
}
