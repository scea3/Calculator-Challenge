using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Services;

var parser = new InputParser();
var calc = new CalculatorService(parser);

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