namespace Task5.Domain
{
    internal class BankAccount
    {
        public Guid Id { get; }
        private decimal _balance;
        public decimal Balance
        {
            get => this._balance;
            private set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Balance cannot be negative");

                this._balance = value;
            }
        }

        public BankAccount(decimal balance)
        {
            this.Id = Guid.NewGuid();
            Balance = balance;
        }

        public void DepositFunds(decimal funds) => this.Balance += funds;
        public void WithdrawFunds(decimal funds) => this.Balance -= funds;
    }
}
