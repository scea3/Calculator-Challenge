using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Rules;

namespace CalculatorChallenge.Core.Services;

public class CalculatorService : ICalculatorService
{
    readonly IInputParser _parser;
    readonly INumberRules _rules;

    public CalculatorService(IInputParser parser, INumberRules rules)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        _rules = rules ?? throw new ArgumentNullException(nameof(rules));
    }

    public int Add(string? input)
    {
        var parsed = _parser.Parse(input);

        var numbers = new int[parsed.Tokens.Count];
        for (int i = 0; i < parsed.Tokens.Count; i++)
        {
            numbers[i] = TryParseIntOrZero(parsed.Tokens[i]);
        }

        var normalized = _rules.Apply(numbers);

        var sum = 0;
        for (int i = 0; i < normalized.Length; i++)
        {
            sum += normalized[i];
        }

        return sum;
    }

    static int TryParseIntOrZero(string token) => int.TryParse(token, out var n) ? n : 0;
}
