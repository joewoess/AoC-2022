using System.Reflection;

namespace aoc_csharp;
public static class Printer
{
    public static (string Message, int Padding)[] ColumnHeaders { get; } = new[] {
        ("Day", Config.InfoColumnPadding),
        ("Type", Config.InfoColumnPadding),
        ("1st", Config.ResultColumnPadding),
        ("2nd", Config.ResultColumnPadding) };

    /** The estimated width of the console window use for printing the seperator line */
    public static int ConsoleWidth { get; set; } = 105;

    /** Try and update the console width. This can fail if the console isn't a real console, e.g. when running emulated */
    public static int TryUpdateConsoleWidth()
    {
        try { ConsoleWidth = Console.WindowWidth; } catch { Printer.DebugMsg($"Could not set console width.  Defaulting to {ConsoleWidth} (Covers early debug logs)"); }
        return ConsoleWidth;
    }

    /** Prints the beginning header of console output */
    public static void PrintGreeting()
    {
        PrintSeparator();
        Console.WriteLine(Config.GreetingMessage);
        PrintSeparator();
    }

    /** Prints the result of a days puzzle in the result table */
    public static void PrintSolutionMessage(int day)
    {
        var impl = Puzzles.PuzzleImplementationDict[day] ?? Puzzles.NoImplementation;
        var firstPuzzle = Config.ShowFirst ? impl.FirstResult : Config.SkippedMessage;
        var secondPuzzle = Config.ShowSecond ? impl.SecondResult : Config.SkippedMessage;

        var columnsToPrint = new (string Message, int Padding)[] {
            (string.Format(Config.DayMessageConvention, day), Config.InfoColumnPadding),
            (impl.TypeName, Config.InfoColumnPadding),
            (firstPuzzle, Config.ResultColumnPadding),
            (secondPuzzle, Config.ResultColumnPadding)
        }.Select(pair => pair.Message.PadLeft(pair.Padding));

        Console.WriteLine($"| {string.Join(" | ", columnsToPrint)} |");
    }

    /** Prints the result of all days in the result table */
    public static void PrintAllSolutionMessages()
    {
        Puzzles.PuzzleImplementationDict.Keys.ToList().ForEach(day => PrintSolutionMessage(day));
    }

    /** Prints the result of the last day with an implementation in the result table */
    public static void PrintLastSolutionMessage()
    {
        var lastDay = Puzzles.PuzzleImplementationDict.Last(entry => entry.Value != Puzzles.NoImplementation).Key;
        Printer.PrintSolutionMessage(lastDay);
    }

    /** Prints a header for the results table */
    public static void PrintResultHeader()
    {
        var columnsHeadersToPrint = ColumnHeaders.Select(header => header.Message.PadLeft(header.Padding));
        Console.WriteLine($"| {string.Join(" | ", columnsHeadersToPrint)} |");
    }

    /** Prints a separator line spanning the console width */
    public static void PrintSeparator(bool onlyDuringDebug = false)
    {
        if (!onlyDuringDebug || Config.IsDebug) Console.WriteLine(new string(Config.LineArtChar, ConsoleWidth));
    }

    /** Prints a message only if the static variable IsDebug is set */
    public static void DebugMsg(string message)
    {
        if (Config.IsDebug) Console.WriteLine(message);
    }

    /** Print a collection only up to a certain amount */
    public static void DebugPrintExcerpt<T>(IEnumerable<T> collection, string? prefix = null, int maxCount = 10, string seperator = ", ")
    {
        if (Config.IsDebug)
        {
            if (prefix != null) Console.Write(prefix);
            var actuallyTaken = collection.Take(maxCount).ToList();
            Console.WriteLine($"{string.Join(seperator, actuallyTaken)} {(actuallyTaken.Count == maxCount ? $"... and {collection.Count() - actuallyTaken.Count} more" : string.Empty)}");
        }
    }
}