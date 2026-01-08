namespace CalculatorChallenge.Core.CustomExceptions;

public class QuantityOfNumberNotAllowedException : ArgumentException
{
    public int Max { get; }

    public QuantityOfNumberNotAllowedException(int max)
        : base(BuildMessage(max))
    {
        Max = max;
    }

    static string BuildMessage(int max) => $"The maximum allowed number of values is {max}.";
}
