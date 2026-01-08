using CalculatorChallenge;
using CalculatorChallenge.Core.Parser;
using CalculatorChallenge.Core.Rules;
using CalculatorChallenge.Core.Services;
using Microsoft.Extensions.DependencyInjection;

var options = AppOptions.Parse(args);
var services = new ServiceCollection();

services.AddSingleton<IInputParser>(_ => new InputParser(options.AlternateDelimiter));
services.AddSingleton<INumberRules>(_ => new NumberRules(options.DenyNegatives, options.UpperBoundInclusive));
services.AddSingleton<CalculatorService>();

var sp = services.BuildServiceProvider();
var calc = sp.GetRequiredService<CalculatorService>();

Console.WriteLine("Enter input lines. Press Ctrl+C to exit.");

Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    Environment.Exit(0);
};

while (true)
{
    var input = Console.ReadLine();
    if (input is null) continue;

    try
    {
        if (options.ShowFormula)
        {
            var detailed = calc.AddDetailed(input);
            Console.WriteLine(detailed.Formula);
        }
        else
        {
            Console.WriteLine(calc.Add(input));
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}