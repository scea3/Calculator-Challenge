namespace CalculatorChallenge.Core.Parser;

public class InputParser : IInputParser
{
    static readonly HashSet<string> DefaultDelimiters = [",", "\n"];

    public ParsedInput Parse(string? input)
    {
        var raw = input ?? string.Empty;

        var tokens = Tokenize(raw);

        return new ParsedInput(DefaultDelimiters, tokens);
    }

    static IReadOnlyList<string> Tokenize(string input)
    {
        if (input.Length == 0)
            return [string.Empty];

        const char sep = '\u001F';
        var normalized = input;

        foreach (var d in DefaultDelimiters)
        {
            normalized = normalized.Replace(d, sep.ToString(), StringComparison.Ordinal);
        }

        return [.. normalized.Split(sep)];
    }
}
