global using aoc_csharp.helper;

Config.IsDemo = args.Contains("--demo");
Config.IsDebug = args.Contains("--debug");
Config.ShowLast = args.Contains("--last");
Config.ShowFirst = !args.Contains("--second") || args.Contains("--first");
Config.ShowSecond = !args.Contains("--first") || args.Contains("--second");


var explicitDaysRequested = args
    .Where(arg => arg.All(char.IsDigit))
    .Select(int.Parse)
    .Where(day => day.Between(1, Config.MaxChallengeDays, true))
    .ToList();

var longestMessage =
    $"IsDemo: {Config.IsDemo} | IsDebug: {Config.IsDebug} | ShowLast: {Config.ShowLast} | ShowFirst: {Config.ShowFirst} | ShowSecond: {Config.ShowSecond} |  ExplicitDays: [{string.Join(",", explicitDaysRequested)}]";

Printer.ConsoleWidth = longestMessage.Length;
if (Config.TryAndUseConsoleWidth) Printer.TryUpdateConsoleWidth();

Printer.PrintGreeting();
Printer.DebugMsg(longestMessage);
Printer.DebugMsg($"Found {Puzzles.ImplementedPuzzles.Count} implementations");
Printer.PrintSeparator(onlyDuringDebug: true);
Printer.PrintResultHeader();

if (explicitDaysRequested.Count > 0) explicitDaysRequested.ForEach(Printer.PrintSolutionMessage);
if (Config.ShowLast) Printer.PrintLastSolutionMessage();
if (explicitDaysRequested.Count == 0 && !Config.ShowLast) Printer.PrintAllSolutionMessages();