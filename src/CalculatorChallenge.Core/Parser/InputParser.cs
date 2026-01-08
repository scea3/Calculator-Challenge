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
            foreach (var d in ExtractBracketDelimiters(header).Where(d => !string.IsNullOrEmpty(d)))
            {
                delimiters.Add(d);
            }
        }
        else
        {
            if (header.Length > 0)
                delimiters.Add(header[0].ToString());
        }        

        return (delimiters, numbersSection);
    }

    static IEnumerable<string> ExtractBracketDelimiters(string header)
    {
        var i = 0;
        while (i < header.Length)
        {
            if (header[i] != '[')
            {
                i++;
                continue;
            }

            var close = header.IndexOf(']', i + 1);
            if (close < 0) yield break;

            yield return header.Substring(i + 1, close - i - 1);
            i = close + 1;
        }
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
