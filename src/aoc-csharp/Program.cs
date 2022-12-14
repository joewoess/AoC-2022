global using aoc_csharp.helper;
using System.Reflection;
using BenchmarkDotNet.Running;

Config.IsDemo = args.Contains("--demo");
Config.IsDebug = args.Contains("--debug");
Config.ShowLast = args.Contains("--last");
Config.ShowFirst = !args.Contains("--second") || args.Contains("--first");
Config.ShowSecond = !args.Contains("--first") || args.Contains("--second");

if (args.Contains("--test"))
{
    Printer.DebugMsg("Running quick coding tests...");
    Test.QuicklyTryCode();
    return;
}

if (args.Contains("--bench"))
{
    Printer.PrintGreeting();
    var isRelease = BenchmarkConfig.IsInReleaseConfiguration();
    Printer.DebugMsg("Started with arg --bench and IsRelease: " + isRelease);
    if (!isRelease)
    {
        Printer.DebugMsg("!!! Benchmarks should only be run in release mode !!!");
        // return;
    }

    var benchmarkClasses = BenchmarkConfig.GetBenchmarks().ToArray();
    Printer.DebugMsg($"Found {benchmarkClasses.Length} benchmarks [{string.Join(", ", benchmarkClasses.Select(t => t.Name))}]");

    var explicitBenchmarkRequested = args
        .Intersect(benchmarkClasses.Select(t => t.Name))
        .ToList();
    if (explicitBenchmarkRequested.Any())
    {
        benchmarkClasses = benchmarkClasses
            .Where(t => explicitBenchmarkRequested.Contains(t.Name))
            .ToArray();
        Printer.DebugMsg($"Running only benchmarks [{string.Join(", ", benchmarkClasses.Select(t => t.Name))}]");
    }

    var summaries = BenchmarkRunner.Run(benchmarkClasses);
    foreach (var summary in summaries)
    {
        var nameOfClass = summary.BenchmarksCases.FirstOrDefault()?.Descriptor?.Type?.Name ?? "Unknown";
        Printer.PrintSeparator();
        Printer.DebugMsg($"Benchmark Summary for {nameOfClass}:");
        BenchmarkConfig.PrintBenchmarkSummary(summary);
    }

    Printer.PrintSeparator();
    Printer.DebugMsg("Done running Benchmarks");

    return;
}

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