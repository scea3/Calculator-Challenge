using System.Runtime.InteropServices;

namespace CalculatorChallenge.Core.Parser;

public class InputParser : IInputParser
{
    static readonly HashSet<string> DefaultDelimiters = [",", "\n"];

    public ParsedInput Parse(string? input)
    {
        var (delimiters, intputParsed) = ParseHeader(input ?? string.Empty);

        var tokens = Tokenize(intputParsed, delimiters);

        return new ParsedInput(delimiters, tokens);
    }

    static (HashSet<string> Delimiters, string IntputParsed) ParseHeader(string input)
    {
        if (!input.StartsWith("//", StringComparison.Ordinal))
        {
            return (DefaultDelimiters, input);
        }

        var newlineIndex = input.IndexOf('\n');
        if (newlineIndex < 0)
        {
            return (DefaultDelimiters, string.Empty);
        }

        var header = input[2..newlineIndex];
        var numbersSection = input[(newlineIndex + 1)..];

        HashSet<string> delimiters = [.. DefaultDelimiters];


        if (header.StartsWith('['))
        {
            var closeIndex = header.IndexOf(']',1);
            if (closeIndex >= 0)
                delimiters.Add(header[1..closeIndex]);
        }
        else
        {
            if (header.Length > 0)
                delimiters.Add(header[0].ToString());
        }        

        return (delimiters, numbersSection);
    }

    static IReadOnlyList<string> Tokenize(string input, HashSet<string> delimiters)
    {
        if (input.Length == 0)
            return [string.Empty];

        const char sep = '\u001F';
        var normalized = input;

        foreach (var d in delimiters)
        {
            normalized = normalized.Replace(d, sep.ToString(), StringComparison.Ordinal);
        }

        return [.. normalized.Split(sep)];
    }
}
