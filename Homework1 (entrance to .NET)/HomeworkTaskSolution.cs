using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal enum TemperatureType
    {
        Celsium,
        Farenheit,
    }

    internal static class HomeworkTaskSolution
    {
        private static string GetSeason(DateTime date)
            => date.Month switch
            {
                12 or 1 or 2 => "Winter",
                3 or 4 or 5 => "Spring",
                6 or 7 or 8 => "Summer",
                9 or 10 or 11 => "Autumn",
                _ => "Unknown"
            };

        public static void CheckFizzBuzzForNumber(int number)
        {
            if (number < 0 || number > 100)
                throw new ArgumentOutOfRangeException("Number must be between 1 and 100");

            string? result = ((number % 3 == 0) ? "Fizz " : null)
                + ((number % 5 == 0) ? "Buzz" : null);
            result ??= number.ToString();

            Console.WriteLine(result);
        }

        public static void PrintPercentFromNumber(int number, int percent)
            => Console.WriteLine((number * percent) / 100);

        public static int CreateFourDigitNumber(int digit1, int digit2, int digit3, int digit4)
            => digit1 * 1000 + digit2 * 100 + digit3 * 10 + digit4;

        public static int ReplaceDigitsInNumber(int sixDigitNumber, int position1, int position2)
        {
            char[] numberParts = sixDigitNumber.ToString().ToArray<char>();

            if (numberParts.Length != 6)
                throw new ArgumentException("Number must contain six digits");

            if (position1 < 0 || position1 >= numberParts.Length ||
                position2 < 0 || position2 >= numberParts.Length)
                throw new IndexOutOfRangeException("Indexes were out of range");

            (numberParts[position1], numberParts[position2]) = (numberParts[position2], numberParts[position1]);
            
            return int.Parse(new string(numberParts));
        }

        public static void PrintSeasonAndDayOfDate(string? stringDate)
        {
            if (DateTime.TryParseExact(stringDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                Console.WriteLine($"{GetSeason(date)} {date.DayOfWeek.ToString()}");
            else
                throw new ArgumentException("User input date doesn`t fit template of date xx.xx.xxxx");
        }

        public static void ConvertAndPrintTemperature(decimal temperature, TemperatureType targetType)
        {
            decimal targetTemperature = targetType switch
            {
                TemperatureType.Celsium => TemperatureConverter.GetFromFarenheitToCelsium(temperature),
                TemperatureType.Farenheit => TemperatureConverter.GetFromCelsiumToFarenheit(temperature),
            };

            Console.WriteLine($"In {targetType}: {targetTemperature}");
        }

        public static void PrintEvenNumbersInRanges(int start, int end)
        {
            if (end < start)
                (start, end) = (end, start);

            for (int i = start; i < end; i++)
                if (i % 2 == 0)
                    Console.WriteLine(i);
        }

        public static bool IsArmstrongNumber(int number)
        {
            int[] numberParts = 
                Array.ConvertAll(number.ToString().ToArray<char>(), 
                element => element - '0');

            return Array.ConvertAll(numberParts, number => Convert.ToInt32(Math.Pow(number, 3))).Sum() == number;
        }

        public static bool IsNumberPerfect(int number)
        {
            int sum = 1;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                {
                    sum += i;
                    int pairDivisor = number / i;

                    if (i != pairDivisor)
                        sum += pairDivisor;
                }
            }

            return sum == number && number != 1;
        }
    }
}
