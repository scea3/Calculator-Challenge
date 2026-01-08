using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Rules;
using CalculatorChallenge.Core.Services;

var parser = new InputParser();
var rules = new NumberRules(true, 1000);
var calc = new CalculatorService(parser, rules);

Console.WriteLine("Enter input string to add (e.g. 1,2)");
var input = Console.ReadLine();

try
{
    var result = calc.Add(input);
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}