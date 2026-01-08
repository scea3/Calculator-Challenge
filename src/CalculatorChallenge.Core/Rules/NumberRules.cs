using CalculatorChallenge.Core.CustomExceptions;

namespace CalculatorChallenge.Core.Rules;

public class NumberRules : INumberRules
{
    public readonly bool DenyNegatives;
    public readonly int UpperBoundInclusive;

    public NumberRules(bool denyNegatives = true, int upperBoundInclusive = 1000)
    {
        DenyNegatives = denyNegatives;
        UpperBoundInclusive = upperBoundInclusive;
    }

    public int[] Apply(int[] numbers)
    {
        if (DenyNegatives)
        {
            var negatives = new List<int>();
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] < 0)
                    negatives.Add(numbers[i]);
            }

            if (negatives.Count > 0)
                throw new NegativeNumbersNotAllowedException(negatives);
        }

        var result = new int[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            var n = numbers[i];
            result[i] = (n > UpperBoundInclusive) ? 0 : n;
        }

        return result;
    }
}
