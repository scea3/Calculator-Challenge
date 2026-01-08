using CalculatorChallenge.Core.Models;

namespace CalculatorChallenge.Core.Parser;

public interface IInputParser
{
    ParsedInput Parse(string? input);
}
