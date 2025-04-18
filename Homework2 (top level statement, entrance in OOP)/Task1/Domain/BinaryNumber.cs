﻿namespace Task1.Domain
{
    internal class BinaryNumber : NumberDecorator
    {
        public BinaryNumber(int value) : base(Convert.ToString(value, 2))
        {

        }

        public BinaryNumber(string value) : base(value)
        {
            if (!IsBinary(value))
                throw new FormatException("Invalid binary format. Only '0' and '1' are allowed");

            if (value.Length > 32)
                throw new ArgumentOutOfRangeException("Value is out of integer limit");
        }

        public static implicit operator int(BinaryNumber binaryInt) => Convert.ToInt32(binaryInt.Value, 2);

        public override int ToInt() => (int)this;

        private static bool IsBinary(string binary) => binary.ToCharArray().All(element => element == '0' || element == '1');
    }
}
