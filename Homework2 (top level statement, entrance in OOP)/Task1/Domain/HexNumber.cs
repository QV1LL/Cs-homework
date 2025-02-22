using System.Globalization;

namespace Task1.Domain
{
    internal class HexNumber : NumberDecorator
    {
        public HexNumber(int value) : base(value.ToString("X"))
        {

        }

        public HexNumber(string value) : base(value)
        {

        }

        public static implicit operator int(HexNumber hexInt)
        {
            string hex = hexInt.Value;

            hex = hex.Trim();

            if (hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                hex = hex.Substring(2);

            return int.Parse(hex, NumberStyles.HexNumber);
        }

        public override int ToInt() => (int)this;
    }
}
