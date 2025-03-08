using System.Drawing;

namespace Task1;

internal delegate Color GettingColor(string? property);

internal static class Program
{
    static void Main(string[] args)
    {
        GettingColor getRGB = (string? name) =>
        {
            if (!Enum.TryParse<RainbowColors>(char.ToUpper(name[0]) + name?.Substring(1), out RainbowColors color))
                throw new ArgumentException("Name does`nt represent a rainbow color");

            return Color.FromName(color.ToString());
        };

        string? colorName = Console.ReadLine();

        Color color = getRGB(colorName);
        Console.WriteLine($"R: {color.R}, G: {color.G}, B: {color.B}");
    }
}
