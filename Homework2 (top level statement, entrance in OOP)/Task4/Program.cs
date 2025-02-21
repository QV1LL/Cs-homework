using System.Text.RegularExpressions;

namespace Task4
{
    internal static class Program
    {
        private const string _inequalityPattern = @"^\s*(-?\d+)\s*(<=|>=|==|!=|<|>)\s*(-?\d+)\s*$";

        static void Main()
        {
            Console.WriteLine("Enter inequality:");
            string? input = Console.ReadLine();

            try
            {
                bool result = EvaluateInequality(input);
                Console.WriteLine("Result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured: " + ex.Message);
            }
        }

        static bool EvaluateInequality(string inequality)
        {
            Match match = Regex.Match(inequality, _inequalityPattern);

            if (!match.Success)
                throw new ArgumentException("Incorrect format of inequality");

            int leftOperand = int.Parse(match.Groups[1].Value);
            string @operator = match.Groups[2].Value;
            int rightOperand = int.Parse(match.Groups[3].Value);

            return @operator switch
            {
                ">" => leftOperand > rightOperand,
                "<" => leftOperand < rightOperand,
                ">=" => leftOperand >= rightOperand,
                "<=" => leftOperand <= rightOperand,
                "==" => leftOperand == rightOperand,
                "!=" => leftOperand != rightOperand,
                _ => throw new ArgumentException($"Unsupported operator: {@operator}")
            };
        }
    }
}