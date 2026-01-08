using CalculatorChallenge.Core.Models;

namespace CalculatorChallenge.Core.Services;

public interface ICalculatorOperation
{
    OperationType Type { get; }
    int Apply(IReadOnlyList<int> terms);
}
