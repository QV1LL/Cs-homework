using System.Collections.Immutable;

namespace NumbersAnalyzer.Services;

internal static class NumbersService
{
	public static int CountUnique(IEnumerable<int> numbers)
	{
		return numbers.ToImmutableHashSet().Count;
	}

	public static int CountMaxIncreasingSequence(IEnumerable<int> numbers)
	{
        ArgumentNullException.ThrowIfNull(numbers);

        if (!numbers.Any())
            return 0;

        int maxSequenceLength = 1;
        int currentSequenceLength = 1;
        int previousNumber = numbers.First();

        foreach (int number in numbers.Skip(1))
        {
            if (previousNumber < number)
            {
                currentSequenceLength++;
                maxSequenceLength = Math.Max(maxSequenceLength, currentSequenceLength);
            }
            else
            {
                currentSequenceLength = 1;
            }

            previousNumber = number;
        }

        return maxSequenceLength;
    }

    public static int CountMaxIncreasingPositiveSequence(IEnumerable<int> numbers)
    {
        var positiveNumbers = numbers.AsParallel()
                                     .Where(x => x >= 0);

        return CountMaxIncreasingSequence(positiveNumbers);
    }


}