using CalculatorChallenge.Core.CustomExceptions;

namespace CalculatorChallenge.Core.Rules;

public class NumberRules : INumberRules
{
    public bool DenyNegatives { get; }

    public NumberRules(bool denyNegatives = true)
    {
        DenyNegatives = denyNegatives;
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

        return numbers;
    }
}
