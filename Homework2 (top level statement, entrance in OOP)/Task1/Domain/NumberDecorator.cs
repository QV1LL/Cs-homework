namespace Task1.Domain
{
    internal abstract class NumberDecorator
    {
        private readonly string _value;

        public string Value
        {
            get => this._value;
        }

        protected NumberDecorator(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Invalid value");

            this._value = value;
        }

        public abstract int ToInt();

        public override string ToString() => this._value;
    }
}
