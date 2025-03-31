using System.Globalization;

namespace Task2.Services;

public static class ExpressionEvaluator
{
    public static string Evaluate(string expression)
    {
        try
        {
            expression = expression.Replace(" ", "");
            if (string.IsNullOrEmpty(expression))
                return "Error!";

            var result = ParseExpression(expression);
            return result.ToString(CultureInfo.InvariantCulture);
        }
        catch
        {
            return "Error!";
        }
    }

    private static double ParseExpression(string expression)
    {
        var values = new Stack<double>();
        var operators = new Stack<char>();
        int i = 0;

        bool checkMinusBeforeFirstNumber = true;

        while (i < expression.Length)
        {
            if (Char.IsDigit(expression[i]) || (checkMinusBeforeFirstNumber && expression[i] == '-'))
            {
                checkMinusBeforeFirstNumber = false;
                bool wasDotInExpression = false;
                double decimalPartOfNumber = 0;
                double wholePartOfNumber = 0;
                double decimalPlaceValue = 1;

                int valueSignCounter = 1;
                if (expression[i] == '-') valueSignCounter = -1;

                    while (i < expression.Length && (Char.IsDigit(expression[i]) || (expression[i] == '.' && !wasDotInExpression)))
                {
                    if (expression[i] == '.' && Char.IsDigit(expression[i + 1]))
                    {
                        i++;
                        wasDotInExpression = true;
                    }
                    else if (wasDotInExpression)
                    {
                        decimalPartOfNumber = decimalPartOfNumber * 10 + (expression[i] - '0');
                        decimalPlaceValue *= 10;
                        i++;
                    }
                    else
                    {
                        wholePartOfNumber = wholePartOfNumber * 10 + (expression[i] - '0');
                        i++;
                    }
                }
                values.Push(valueSignCounter * (wholePartOfNumber + decimalPartOfNumber / decimalPlaceValue));
            }
            else if (expression[i] == '+' || expression[i] == '-')
            {
                while (operators.Count > 0 && (operators.Peek() == '*' || operators.Peek() == '/'))
                {
                    values.Push(ApplyOperator(operators.Pop(), values.Pop(), values.Pop()));
                }
                operators.Push(expression[i]);
                i++;
            }
            else if (expression[i] == '*' || expression[i] == '/')
            {
                operators.Push(expression[i]);
                i++;
            }
            else
            {
                throw new InvalidOperationException("Invalid character in expression.");
            }
        }

        while (operators.Count > 0)
        {
            values.Push(ApplyOperator(operators.Pop(), values.Pop(), values.Pop()));
        }

        return values.Pop();
    }

    private static double ApplyOperator(char op, double b, double a)
    {
        switch (op)
        {
            case '+': return a + b;
            case '-': return a - b;
            case '*': return a * b;
            case '/': return a / b;
            default: throw new InvalidOperationException("Unsupported operator");
        }
    }
}
