using Task5.Domain;

namespace Task5
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter a start balance: ");
                decimal balance = Convert.ToDecimal(Console.ReadLine());

                BankAccount account = new BankAccount(balance);

                Console.WriteLine("Deposit some funds to your account: ");
                decimal deposit = Convert.ToDecimal(Console.ReadLine());

                account.DepositFunds(deposit);

                Console.WriteLine("Withdraw some funds from your account: ");
                decimal withdraw = Convert.ToDecimal(Console.ReadLine());

                account.WithdrawFunds(withdraw);

                Console.WriteLine($"Current balance: {account.Balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }
        }
    }
}