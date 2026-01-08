namespace CalculatorChallenge.Core.Formatters;

public static class FormulaFormatter
{
    public static string Format(IReadOnlyList<int> terms, int sum) => $"{string.Join("+", terms)} = {sum}";
}
