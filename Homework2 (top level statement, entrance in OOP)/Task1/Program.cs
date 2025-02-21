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
            string? currentNumberSystemString = Console.ReadLine();

            if (!Enum.TryParse(currentNumberSystemString, out NumberSystem currentNumberSystem))
            {
                Console.WriteLine("Wrong number system!");
                return;
            }

            IntDecorator? numberWrapped;

            try
            {
                numberWrapped = currentNumberSystem switch
                {
                    NumberSystem.Binary => new BinaryInt(number),
                    NumberSystem.Hex => new HexInt(number),
                    NumberSystem.Decimal => new Int(number)
                };
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return;
            }

            Console.WriteLine("Enter target number system (binary, hex, decimal): ");
            string? targetNumberSystemString = Console.ReadLine();

            if (!Enum.TryParse(targetNumberSystemString, out NumberSystem targetNumberSystem))
            {
                Console.WriteLine("Wrong number system!");
                return;
            }

            IntDecorator result = targetNumberSystem switch
            {
                NumberSystem.Binary => new BinaryInt(numberWrapped.ToInt()),
                NumberSystem.Hex => new HexInt(numberWrapped.ToInt()),
                NumberSystem.Decimal => new Int(numberWrapped.ToInt())
            };

            Console.WriteLine($"Number in {targetNumberSystem.ToString().ToLower()}: {result}");
        }
    }
}