namespace Task1.Domain
{
    internal abstract class IntDecorator
    {
        private readonly string _value;

        public string Value
        {
            get => this._value;
        }

        protected IntDecorator(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Invalid value");

            this._value = value;
        }

        public abstract int ToInt();

        public override string ToString() => this._value;
    }
}
