namespace CalculatorChallenge.Core.CustomExceptions;

public class NegativeNumbersNotAllowedException : ArgumentException
{
    public IReadOnlyList<int> Negatives { get; }

    public NegativeNumbersNotAllowedException(IEnumerable<int> negatives)
        : base(BuildMessage(negatives))
    {
        Negatives = [.. negatives];
    }

    static string BuildMessage(IEnumerable<int> negatives) => "Negatives not allowed: " + string.Join(",", negatives);
}
