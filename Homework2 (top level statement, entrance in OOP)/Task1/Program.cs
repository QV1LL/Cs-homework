using Task1.Domain;

namespace Task1
{
    internal enum NumberSystem
    {
        Binary,
        Hex,
        Decimal,
    }
    internal static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter number: ");
            string? number = Console.ReadLine();

            Console.WriteLine("Enter current number system (binary, hex, decimal): ");
            string? currentNumberSystemString = Capitalize(Console.ReadLine());

            if (!Enum.TryParse(currentNumberSystemString, out NumberSystem currentNumberSystem))
            {
                Console.WriteLine("Wrong number system!");
                return;
            }

            NumberDecorator? numberWrapped;

            try
            {
                numberWrapped = currentNumberSystem switch
                {
                    NumberSystem.Binary => new BinaryNumber(number),
                    NumberSystem.Hex => new HexNumber(number),
                    NumberSystem.Decimal => new DecimalNumber(number)
                };
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return;
            }

            Console.WriteLine("Enter target number system (binary, hex, decimal): ");
            string? targetNumberSystemString = Capitalize(Console.ReadLine());

            if (!Enum.TryParse(targetNumberSystemString, out NumberSystem targetNumberSystem))
            {
                Console.WriteLine("Wrong number system!");
                return;
            }

            NumberDecorator result = targetNumberSystem switch
            {
                NumberSystem.Binary => new BinaryNumber(numberWrapped.ToInt()),
                NumberSystem.Hex => new HexNumber(numberWrapped.ToInt()),
                NumberSystem.Decimal => new DecimalNumber(numberWrapped.ToInt())
            };

            Console.WriteLine($"Number in {targetNumberSystem.ToString().ToLower()}: {result}");
        }

        private static string? Capitalize(string? text)
        {
            if (text == null) return null;

            return text[0].ToString().ToUpper() + ((text.Length > 1) ? text.Substring(1).ToLower() : string.Empty);
        }
    }
}