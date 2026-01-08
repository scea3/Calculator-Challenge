using CalculatorChallenge.Core.Parser;

namespace CalculatorChallenge.Core.Services;

public class CalculatorService : ICalculatorService
{
    readonly IInputParser _parser;

    public CalculatorService(IInputParser parser)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    }

    public int Add(string? input)
    {
        var parsed = _parser.Parse(input);

        var sum = 0;
        for (int i = 0; i < parsed.Tokens.Count; i++)
        {
            sum += TryParseIntOrZero(parsed.Tokens[i]);
        }

        return sum;
    }

    static int TryParseIntOrZero(string token) => int.TryParse(token, out var n) ? n : 0;
}
