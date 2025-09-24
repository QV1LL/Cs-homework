namespace TaskFourAndFive.Services;

internal class NumbersService
{
    public const string PATH_TO_WRITE_RANDOM_NUMBERS = "random_numbers.txt";
    public const string PATH_TO_WRITE_PRIME_NUMBERS = "prime_numbers.txt";
    public const string PATH_TO_WRITE_ENDS_WITH_7_NUMBERS = "ends_with_7_numbers.txt";

    private readonly Random _random;

    public NumbersService()
    {
        _random = new Random();
    }

    public async Task GenerateAndWriteNumbers()
    {
        List<int> numbers = [.. Enumerable.Range(1, 100).Select(_ => _random.Next())];
        await FileStorageService.WriteNumbersToFile(numbers, PATH_TO_WRITE_RANDOM_NUMBERS);
    }

    public async Task WritePrimeNumbers()
    {
        var numbers = await FileStorageService.ParseNumbersFromFile(PATH_TO_WRITE_RANDOM_NUMBERS);
        await FileStorageService.WriteNumbersToFile(numbers.Where(IsPrime), PATH_TO_WRITE_PRIME_NUMBERS);
    }

    public async Task WriteNumbersEndsWith7()
    {
        var numbers = await FileStorageService.ParseNumbersFromFile(PATH_TO_WRITE_PRIME_NUMBERS);
        await FileStorageService.WriteNumbersToFile(numbers.Where(n => n.ToString().EndsWith('7')), PATH_TO_WRITE_ENDS_WITH_7_NUMBERS);
    }

    private static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number <= 3) return true;
        if (number % 2 == 0 || number % 3 == 0) return false;
        
        for (int i = 5; i * i <= number; i += 6)
            if (number % i == 0 || number % (i + 2) == 0)
                return false;

        return true;
    }
}
