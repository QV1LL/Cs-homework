using static System.Net.Mime.MediaTypeNames;

namespace Task2
{
    internal static class Program
    {
        internal enum Number
        {
            Zero,
            One,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eigth,
            Nine,
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number in string: ");
            string? numberString = Console.ReadLine();
            
            if(numberString == null || numberString.Length < 2)
            {
                Console.WriteLine("Invalid input");
                return;
            }

            numberString = numberString.Substring(0, 1).ToUpper() + numberString.Substring(1);

            if (!Enum.TryParse(numberString, out Number number))
            {
                Console.WriteLine("Invalid input");
                return;
            }

            Console.WriteLine((int)number);
        }
    }
}