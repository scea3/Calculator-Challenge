namespace CalculatorChallenge.Core.Models;

public sealed record CalculationResult(IReadOnlyList<int> Terms, int Sum, string Formula);