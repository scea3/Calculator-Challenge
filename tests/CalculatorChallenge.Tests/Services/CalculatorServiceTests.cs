using CalculatorChallenge.Core.CustomExceptions;
using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Rules;
using CalculatorChallenge.Core.Services;

namespace CalculatorChallenge.Tests.Services;

public class CalculatorServiceTests
{
    private static CalculatorService CreateCalculator(bool denyNegatives = false, int upperBoundInclusive = 1000)
    {
        return new CalculatorService(
            new InputParser(),
            new NumberRules(denyNegatives, upperBoundInclusive)
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
        var calc = CreateCalculator(upperBoundInclusive: 5000);
        Assert.Equal(expected, calc.Add(input));
    }

    [Fact]
    public void RemovesMaxConstraint_SumsMany()
    {
        var calc = CreateCalculator();
        Assert.Equal(78, calc.Add("1,2,3,4,5,6,7,8,9,10,11,12"));
    }

    [Fact]
    public void SupportsNewlineDelimiter()
    {
        var calc = CreateCalculator();
        Assert.Equal(6, calc.Add("1\n2,3"));
    }

    [Fact]
    public void DeniesNegatives_IncludesAllNegativesInMessage()
    {
        var calc = CreateCalculator(true);

        var ex = Assert.Throws<NegativeNumbersNotAllowedException>(() => calc.Add("1,-2,-3,4"));
        Assert.Contains("-2", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Theory]
    [InlineData("2,1001,6", 8)]
    [InlineData("1000,1", 1001)]
    [InlineData("1002", 0)]
    public void ValuesGreaterThan1000AreInvalid(string input, int expected)
    {
        var calc = CreateCalculator(false, 1000);
        Assert.Equal(expected, calc.Add(input));
    }

    [Theory]
    [InlineData("//#\n2#5", 7)]
    [InlineData("//,\n2,ff,100", 102)]
    public void CustomDelimiterStillSupportsPreviousDelimiters(string input, int expected)
    {
        var calc = CreateCalculator();
        Assert.Equal(expected, calc.Add(input));
    }

    [Fact]
    public void CustomDelimiterAnyLength()
    {
        var calc = CreateCalculator();
        Assert.Equal(66, calc.Add("//[***]\n11***22***33"));
    }

    [Fact]
    public void MultipleDelimitersAnyLength()
    {
        var calc = CreateCalculator();
        Assert.Equal(110, calc.Add("//[*][!!][r9r]\n11r9r22*hh*33!!44"));
    }
}
