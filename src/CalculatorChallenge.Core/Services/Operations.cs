using CalculatorChallenge.Core.Models;

namespace CalculatorChallenge.Core.Services;

public sealed class AddOperation : ICalculatorOperation
{
    public OperationType Type => OperationType.Add;

    public int Apply(IReadOnlyList<int> terms)
    {
        var sum = 0;
        for (int i = 0; i < terms.Count; i++) sum += terms[i];
        return sum;
    }
}

public sealed class SubtractOperation : ICalculatorOperation
{
    public OperationType Type => OperationType.Subtract;

    public int Apply(IReadOnlyList<int> terms)
    {
        if (terms.Count == 0) return 0;
        var result = terms[0];
        for (int i = 1; i < terms.Count; i++) result -= terms[i];
        return result;
    }
}

public sealed class MultiplyOperation : ICalculatorOperation
{
    public OperationType Type => OperationType.Multiply;

    public int Apply(IReadOnlyList<int> terms)
    {
        if (terms.Count == 0) return 0;
        var result = 1;
        for (int i = 0; i < terms.Count; i++) result *= terms[i];
        return result;
    }
}

public sealed class DivideOperation : ICalculatorOperation
{
    public OperationType Type => OperationType.Divide;

    public int Apply(IReadOnlyList<int> terms)
    {
        if (terms.Count == 0) return 0;
        var result = terms[0];

        for (int i = 1; i < terms.Count; i++)
        {
            if (terms[i] == 0)
                throw new DivideByZeroException("Cannot divide by zero term.");
            result /= terms[i];
        }

        return result;
    }
}
