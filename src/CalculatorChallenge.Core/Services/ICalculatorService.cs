using CalculatorChallenge.Core.Models;

namespace CalculatorChallenge.Core.Services;

public interface ICalculatorService
{
    int Add(string? input);
    CalculationResult AddDetailed(string? input);
    CalculationResult CalculateDetailed(string? input, ICalculatorOperation operation);
}
