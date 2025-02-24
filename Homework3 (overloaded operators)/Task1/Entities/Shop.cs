using Task1.Aggregates;

namespace Task1.Entities;

internal class Shop(double square) : IComparable<Shop>, IEquatable<Shop>
{
    public double Square
    {
        get => field;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("Square of shop must be positive value");

            field = value;
        }
    } = square;

    public int CompareTo(Shop? other)
    {
        if (other == null) return 1;
        return this.Square.CompareTo(other.Square);
    }

    public bool Equals(Shop? other) => this.Square.Equals(other?.Square);

    public override bool Equals(object? obj) => Equals(obj as Shop);

    public override int GetHashCode() => this.Square.GetHashCode();

    public static bool operator <(Shop shop1, Shop shop2)
        => shop1.Square < shop2.Square;

    public static bool operator >(Shop shop1, Shop shop2)
        => !(shop1 < shop2);

    public static bool operator <=(Shop shop1, Shop shop2)
        => shop1.Square <= shop2.Square;

    public static bool operator >=(Shop shop1, Shop shop2)
        => !(shop1 <= shop2);

    public static bool operator ==(Shop? shop1, Shop? shop2)
        => shop1?.Square == shop2?.Square;

    public static bool operator !=(Shop? shop1, Shop? shop2) => !(shop1 == shop2);

    public static Shop operator +(Shop shop, double square) => new Shop(shop.Square + square);

    public static Shop operator -(Shop shop, double square) => new Shop(shop.Square - square);
}
