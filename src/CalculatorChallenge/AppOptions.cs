namespace CalculatorChallenge;

public sealed record AppOptions
{
    public string AlternateDelimiter { get; init; } = "\n";
    public bool DenyNegatives { get; init; } = true;
    public int UpperBoundInclusive { get; init; } = 1000;
    public bool ShowFormula { get; init; } = false;

    public static AppOptions Parse(string[] args)
    {
        var opt = new AppOptions();

        foreach (var arg in args)
        {
            if (arg.StartsWith("--alt-delim=", StringComparison.OrdinalIgnoreCase))
            {
                opt = opt with { AlternateDelimiter = Value(arg) };
            }
            else if (arg.StartsWith("--deny-negatives=", StringComparison.OrdinalIgnoreCase))
            {
                opt = opt with { DenyNegatives = BoolValue(arg) };
            }
            else if (arg.StartsWith("--upper=", StringComparison.OrdinalIgnoreCase))
            {
                opt = opt with { UpperBoundInclusive = IntValue(arg) };
            }
            else if (arg.StartsWith("--formula=", StringComparison.OrdinalIgnoreCase))
            {
                opt = opt with { ShowFormula = BoolValue(arg) };
            }
        }

        return opt;

        static string Value(string a) => a[(a.IndexOf('=') + 1)..].Trim().Trim('"');
        static bool BoolValue(string a) => bool.TryParse(Value(a), out var b) && b;
        static int IntValue(string a) => int.TryParse(Value(a), out var n) ? n : 1000;
    }
}
