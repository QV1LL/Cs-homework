namespace Task1;

internal static class Program
{
    public static void Main(string[] args)
    {
        var numbers = Enumerable.Range(1, 100).ToArray();

        Predicate<int> isPrime = number =>
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            for (int i = 3; i <= Math.Sqrt(number); i += 2)
            {
                if (number % i == 0) return false;
            }
            return true;
        };

        Predicate<int> isFibonacci = number =>
        {
            if (number < 0) return false;
            int check1 = 5 * number * number + 4;
            int check2 = 5 * number * number - 4;
            return IsPerfectSquare(check1) || IsPerfectSquare(check2);
        };

        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "prime_numbers.txt"), String.Join(" ", numbers.Where(n => isPrime(n))));
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "fibonacci_numbers.txt"), String.Join(" ", numbers.Where(n => isFibonacci(n))));
    }

    private static bool IsPerfectSquare(int num)
    {
        int sqrt = (int)Math.Sqrt(num);
        return sqrt * sqrt == num;
    }
}
