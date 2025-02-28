namespace Task2.Entities
{
    internal abstract class Vehicle : Device
    {
        public int WorkDelay
        {
            get => field;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Delay must be above 0");

                field = value;
            }
        }

        public Vehicle(string title, string description, string soundKey, int workDelay) : base(title, description, soundKey)
        {
            WorkDelay = workDelay;
        }
    }
}
