using System.Numerics;

namespace Task1;

internal static class ArrayOperations<T> where T : INumber<T>
{
    public static T[] GetEvenArray(T[] array) 
        => array.Where(value => value % T.CreateChecked(2) == T.Zero).ToArray();
    
    public static T[] GetOddArray(T[] array)
        => array.Where(value => value % T.CreateChecked(2) != T.Zero).ToArray();

    public static T[] GetSimpleValuesArray(T[] array)
        => array.Where(value => IsSimple(value)).ToArray();

    public static T[] GetFibonacciValuesArray(T[] array)
        => array.Where(value => IsFibonacci(value)).ToArray();

    private static bool IsSimple(T value)
    {
        if (value <= T.CreateChecked(1)) return false;

        for (T i = T.CreateChecked(2); i < value; i += T.CreateChecked(1))
            if (value % i == T.Zero)
                return false;

        return true;
    }

    private static bool IsFibonacci(T value)
    {
        if (value < T.Zero) return false;

        T five = T.CreateChecked(5);
        T four = T.CreateChecked(4);
        T nSquared = value * value;

        T check1 = five * nSquared + four;
        T check2 = five * nSquared - four;

        return IsPerfectSquare(check1) || IsPerfectSquare(check2);
    }

    private static bool IsPerfectSquare(T value)
    {
        if (value < T.Zero) return false;

        T sqrt = T.CreateChecked((int)Math.Sqrt(double.CreateChecked(value)));
        
        return sqrt * sqrt == value;
    }
}
