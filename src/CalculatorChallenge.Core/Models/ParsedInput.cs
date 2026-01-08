namespace CalculatorChallenge.Core.Models;

public sealed record ParsedInput(HashSet<string> Delimiters, IReadOnlyList<string> Tokens);
