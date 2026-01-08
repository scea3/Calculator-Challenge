namespace CalculatorChallenge.Core.Parser;

public sealed record ParsedInput(HashSet<string> Delimiters, IReadOnlyList<string> Tokens);
