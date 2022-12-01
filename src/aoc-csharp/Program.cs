using aoc_csharp;

Printer.PrintGreeting();
Config.IsDemo = args.Contains("--demo");
Config.IsDebug = args.Contains("--debug");


var implementations = Puzzles.GetImplementedTypesFromNamespace();
if (args.Contains("--last"))
{
    Printer.DebugMsg("Only show last entry (cmd arg --last)");
    Printer.PrintSolutionMessage(implementations.Last());
}
else if (args.Length > 0 && int.TryParse(args[0], out var newNum) && newNum is > 0 and <= 25)
{
    var typeFromNumber = implementations.ElementAtOrDefault(newNum - 1);
    Printer.DebugMsg($"Only show entry {newNum} (cmd arg NUMBER)");
    if (typeFromNumber is not null) Printer.PrintSolutionMessage(typeFromNumber);
}
else
{
    Printer.PrintResultHeader();
    foreach (var targetType in implementations)
    {
        Printer.DebugMsg(targetType.Name);
        try
        {
            Printer.PrintSolutionMessage(targetType);
        }
        catch (Exception e)
        {
            Printer.DebugMsg(e.Message);
            Console.WriteLine($"Could not find solution for day {targetType.Name}");
        }
    }
}