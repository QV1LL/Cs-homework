namespace Homework1
{
    internal static class Program
    {
        public static void Main()
        {
            TestHomeworkSolution();
        }

        public static void TestHomeworkSolution()
        {
            try
            {
                if (int.TryParse(Console.ReadLine(), out int number))
                    HomeworkTaskSolution.CheckFizzBuzzForNumber(number);

                if (int.TryParse(Console.ReadLine(), out number) &&
                    int.TryParse(Console.ReadLine(), out int percent))
                    HomeworkTaskSolution.PrintPercentFromNumber(number, percent);

                if (int.TryParse(Console.ReadLine(), out int digit1) &&
                    int.TryParse(Console.ReadLine(), out int digit2) &&
                    int.TryParse(Console.ReadLine(), out int digit3) &&
                    int.TryParse(Console.ReadLine(), out int digit4))
                    Console.WriteLine("Four digit number: " + HomeworkTaskSolution.CreateFourDigitNumber(digit1, digit2, digit3, digit4));

                if (int.TryParse(Console.ReadLine(), out number) &&
                    int.TryParse(Console.ReadLine(), out int position1) &&
                    int.TryParse(Console.ReadLine(), out int position2))
                    Console.WriteLine("New six digit number: " + HomeworkTaskSolution.ReplaceDigitsInNumber(number, position1, position2));

                HomeworkTaskSolution.PrintSeasonAndDayOfDate(Console.ReadLine());

                if (int.TryParse(Console.ReadLine(), out int temperature) &&
                    TemperatureType.TryParse(Console.ReadLine(), out TemperatureType type))
                    HomeworkTaskSolution.ConvertAndPrintTemperature(temperature, type);

                if (int.TryParse(Console.ReadLine(), out int start) &&
                    int.TryParse(Console.ReadLine(), out int end))
                    HomeworkTaskSolution.PrintEvenNumbersInRanges(start, end);

                if (int.TryParse(Console.ReadLine(), out number))
                    Console.WriteLine($"Is {number} armstrong: {HomeworkTaskSolution.IsArmstrongNumber(number)}");

                if (int.TryParse(Console.ReadLine(), out number))
                    Console.WriteLine($"Is {number} perfect: {HomeworkTaskSolution.IsNumberPerfect(number)}");
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}