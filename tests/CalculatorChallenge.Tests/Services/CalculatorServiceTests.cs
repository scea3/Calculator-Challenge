using CalculatorChallenge.Core.CustomExceptions;
using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Rules;
using CalculatorChallenge.Core.Services;

namespace CalculatorChallenge.Tests.Services;

public class CalculatorServiceTests
{
    private static CalculatorService CreateCalculator(
        bool denyNegatives = false,
        int upperBoundInclusive = 1000,
        string alternateDelimiter = "\n")
    {
        return new CalculatorService(
            new InputParser(alternateDelimiter),
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

    [Fact]
    public void FormulaDetailed()
    {
        var calc = CreateCalculator();
        var result = calc.AddDetailed("2,,4,rrrr,1001,6");
        Assert.Equal(12, result.Sum);
        Assert.Equal("2+0+4+0+0+6 = 12", result.Formula);
    }

    [Fact]
    public void Add_Detailed_IncludesZerosForMissingAndInvalidAndUpperBound()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("2,,4,rrrr,1001,6", new AddOperation());

        Assert.Equal([2, 0, 4, 0, 0, 6], result.Terms);
        Assert.Equal(12, result.Sum);
        Assert.Equal("2+0+4+0+0+6 = 12", result.Formula);
    }

    [Fact]
    public void Subtract_Detailed_FormatsFormulaWithMinus()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("10,3,2", new SubtractOperation());

        Assert.Equal([10, 3, 2], result.Terms);
        Assert.Equal(5, result.Sum);
        Assert.Equal("10-3-2 = 5", result.Formula);
    }

    [Fact]
    public void Multiply_Detailed_FormatsFormulaWithAsterisk()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("2,3,4", new MultiplyOperation());

        Assert.Equal([2, 3, 4], result.Terms);
        Assert.Equal(24, result.Sum);
        Assert.Equal("2*3*4 = 24", result.Formula);
    }

    [Fact]
    public void Divide_Detailed_FormatsFormulaWithSlash()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("100,5,2", new DivideOperation());

        Assert.Equal([100, 5, 2], result.Terms);
        Assert.Equal(10, result.Sum);
        Assert.Equal("100/5/2 = 10", result.Formula);
    }

    [Fact]
    public void Divide_Detailed_ThrowsDivideByZero_WhenAnyTermIsZero()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var ex = Assert.Throws<DivideByZeroException>(() =>
            calc.CalculateDetailed("10,0,2", new DivideOperation()));

        Assert.Contains("divide by zero", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Detailed_StillSupportsNewlineDelimiter()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("1\n2,3", new AddOperation());

        Assert.Equal([1, 2, 3], result.Terms);
        Assert.Equal(6, result.Sum);
        Assert.Equal("1+2+3 = 6", result.Formula);
    }

    [Fact]
    public void Detailed_StillSupportsSingleCharCustomDelimiter()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("//#\n2#5#ff", new AddOperation());

        Assert.Equal([2, 5, 0], result.Terms);
        Assert.Equal(7, result.Sum);
        Assert.Equal("2+5+0 = 7", result.Formula);
    }

    [Fact]
    public void Detailed_StillSupportsMultiDelimiterAnyLength()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false);

        var result = calc.CalculateDetailed("//[*][!!][r9r]\n11r9r22*hh*33!!44", new AddOperation());

        Assert.Equal(new[] { 11, 22, 0, 33, 44 }, result.Terms);
        Assert.Equal(110, result.Sum);
        Assert.Equal("11+22+0+33+44 = 110", result.Formula);
    }

    [Fact]
    public void Detailed_ThrowsWhenNegativesNotAllowed_IncludesAllNegatives()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: true);

        var ex = Assert.Throws<NegativeNumbersNotAllowedException>(() =>
            calc.CalculateDetailed("1,-2,-3,4", new AddOperation()));

        Assert.Contains("-2", ex.Message);
        Assert.Contains("-3", ex.Message);
    }

    [Fact]
    public void Detailed_UsesConfiguredUpperBound()
    {
        var calc = CreateCalculator(upperBoundInclusive: 10, denyNegatives: false);

        var result = calc.CalculateDetailed("9,10,11,12", new AddOperation());

        Assert.Equal([9, 10, 0, 0], result.Terms);
        Assert.Equal(19, result.Sum);
        Assert.Equal("9+10+0+0 = 19", result.Formula);
    }

    [Fact]
    public void Detailed_UsesAlternateDelimiterFromOptions()
    {
        var calc = CreateCalculator(upperBoundInclusive: 1000, denyNegatives: false, alternateDelimiter: ";");

        var result = calc.CalculateDetailed("1;2,3", new AddOperation());

        Assert.Equal([1, 2, 3], result.Terms);
        Assert.Equal(6, result.Sum);
        Assert.Equal("1+2+3 = 6", result.Formula);
    }
}
