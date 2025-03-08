namespace Task7;

internal static class Program
{
    private const string pinCode = "1234";
    private static int balance = 1200;

    public static Predicate<string?> IsEqualsToPinCode = value => value == pinCode;
    public static Action PrintBalance = () => Console.WriteLine(balance);
    public static Action<int> Withdraw = ammount =>
    {
        if (ammount > balance)
            Console.WriteLine("not enough money on balance to continue this operation");

        if (ammount <= 0)
            throw new ArgumentOutOfRangeException("Ammount cannot be lower or equal to zero");

        balance -= ammount;
    };


    public static void Main(string[] args)
    {
        Console.WriteLine("Enter pin code: ");
        string? userInputPinCode = Console.ReadLine();

        if (!IsEqualsToPinCode(userInputPinCode))
            Console.WriteLine("Pin code is invalid");
        
        RunMenu();
    }

    private static void RunMenu()
    {
        string? choice;

        while (true)
        {
            Console.WriteLine("\n*-- Main menu --*");
            Console.WriteLine("1. Print balance");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Exit");

            Console.WriteLine("Choose operation (1-3): ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    PrintBalance();
                    break;

                case "2":
                    Withdraw(Convert.ToInt32(Console.ReadLine()));
                    break;

                case "3":
                    break;

                default:
                    Console.WriteLine("Invalid input: " + choice);
                    break;

            }
        }
    }
}
