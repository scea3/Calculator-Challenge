namespace CalculatorChallenge.Core.Parser;

public interface IInputParser
{
    ParsedInput Parse(string? input);
}
