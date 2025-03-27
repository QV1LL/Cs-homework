using System.Globalization;

namespace Task1to3.ValueObjects;

internal record Money : IComparable<Money>
{
    public int WholePart
    {
        get => field;
        init
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("Whole part must be non negative");

            field = value;
        }
    }

    public int DecimalPart
    {
        get => field;
        init
        {
            if (value < 0 || value > 99)
                throw new ArgumentOutOfRangeException("Decimal part must be in range from 0 to 99");

            field = value;
        }
    }

    public Money(int wholePart, int decimalPart)
    {
        WholePart = wholePart;
        DecimalPart = decimalPart;
    }

    public static Money operator +(Money left, Money right)
    {
        long totalCents = (long)left.WholePart * 100 + left.DecimalPart +
                         (long)right.WholePart * 100 + right.DecimalPart;

        if (totalCents < 0)
            throw new InvalidOperationException("Result cannot be negative");

        var (whole, decimalPart) = Normalize(totalCents);
        return new Money(whole, decimalPart);
    }

    public static Money operator -(Money left, Money right)
    {
        long totalCents = (long)left.WholePart * 100 + left.DecimalPart -
                         ((long)right.WholePart * 100 + right.DecimalPart);
        if (totalCents < 0)
            throw new InvalidOperationException("Result cannot be negative");

        var (whole, decimalPart) = Normalize(totalCents);
        return new Money(whole, decimalPart);
    }

    public static Money operator *(Money money, double factor)
    {
        if (factor < 0)
            throw new InvalidOperationException("Factor cannot be negative");

        long totalCents = (long)(((long)money.WholePart * 100 + money.DecimalPart) * factor);
        if (totalCents < 0)
            throw new InvalidOperationException("Result cannot be negative");

        var (whole, decimalPart) = Normalize(totalCents);
        return new Money(whole, decimalPart);
    }

    public static Money operator /(Money money, double divisor)
    {
        if (divisor <= 0)
            throw new InvalidOperationException("Divisor must be positive");

        long totalCents = (long)(((long)money.WholePart * 100 + money.DecimalPart) / divisor);
        if (totalCents < 0)
            throw new InvalidOperationException("Result cannot be negative");

        var (whole, decimalPart) = Normalize(totalCents);
        return new Money(whole, decimalPart);
    }

    public static Money operator *(double factor, Money money) => money * factor;

    public static bool operator <(Money left, Money right) =>
        (left.WholePart * 100 + left.DecimalPart) < (right.WholePart * 100 + right.DecimalPart);

    public static bool operator >(Money left, Money right) => right < left;

    public static bool operator <=(Money left, Money right) => !(left > right);

    public static bool operator >=(Money left, Money right) => !(left < right);

    public int CompareTo(Money? other)
    {
        if (other is null) return 1; 
        return (WholePart * 100 + DecimalPart).CompareTo(other.WholePart * 100 + other.DecimalPart);
    }

    public override string ToString()
    {
        var regionInfo = new RegionInfo(CultureInfo.CurrentCulture.Name);
        var numberFormat = CultureInfo.CurrentCulture.NumberFormat;

        return $"{WholePart}{numberFormat.NumberDecimalSeparator}{DecimalPart}{regionInfo.CurrencySymbol}";
    }

    private static (int whole, int decimalPart) Normalize(long totalCents)
    {
        int whole = (int)(totalCents / 100);
        int decimalPart = (int)(totalCents % 100);

        if (decimalPart < 0)
        {
            whole--;
            decimalPart += 100;
        }

        return (whole, decimalPart);
    }
}
