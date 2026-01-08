using CalculatorChallenge.Core.CustomExceptions;
using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Services;

namespace CalculatorChallenge.Tests.Services;

public class CalculatorServiceTests
{
    private static CalculatorService CreateCalculator()
    {
        return new CalculatorService(
            new InputParser()
        );
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData("20", 20)]
    [InlineData(",", 0)]
    [InlineData("1,", 1)]
    [InlineData(",2", 2)]
    [InlineData("1,5000", 5001)]
    [InlineData("5,tytyt", 5)]
    [InlineData("4,-3", 1)]
    public void BaseCases(string? input, int expected)
    {
        var calc = CreateCalculator();
        Assert.Equal(expected, calc.Add(input));
    }

    [Fact]
    public void MaxOfNumber_ShouldFail()
    {
        var calc = CreateCalculator();
        Assert.Throws<QuantityOfNumberNotAllowedException>(() => calc.Add("1,2,3"));
    }
}
