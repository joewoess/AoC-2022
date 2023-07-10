namespace aoc_csharp.helper;

public static class Printer
{
    /** Column headers for the results table */
    private static IEnumerable<(string Message, int Padding)> ColumnHeaders { get; } = new[]
    {
        ("Day", Config.InfoColumnPadding),
        ("Type", Config.InfoColumnPadding),
        ("1st", Config.ResultColumnPadding),
        ("2nd", Config.ResultColumnPadding)
    };

    /** The estimated width of the console window use for printing the separator line */
    public static int ConsoleWidth { get; set; } = 105;

    /** Try and update the console width. This can fail if the console isn't a real console, e.g. when running emulated */
    public static bool TryUpdateConsoleWidth()
    {
        try
        {
            ConsoleWidth = Console.WindowWidth;
            return true;
        }
        catch
        {
            DebugMsg($"Could not set console width.  Defaulting to {ConsoleWidth} (Covers early debug logs)");
        }

        return false;
    }

    /** Prints the beginning header of console output */
    public static void PrintGreeting()
    {
        PrintSeparator();
        var lines = Config.GreetingMessageLines;
        var longestLine = lines.Max(line => line.Length);
        var (padLeft, padRight) = ((ConsoleWidth - longestLine) / 2, (ConsoleWidth - 3 + longestLine) / 2);

        Console.WriteLine(string.Join(Environment.NewLine, lines.Select(line => $"|{new string(' ', padLeft)}{line.PadRight(padRight)}|")));
        PrintSeparator();
    }

    /** Prints the result of a days puzzle in the result table */
    public static void PrintSolutionMessage(int day)
    {
        var implementationsOfDay = Puzzles.PuzzleImplementationDict()[day];
        foreach (var impl in implementationsOfDay)
        {
            var firstPuzzle = Config.ShowFirst && !(Config.SkipLongRunning && impl.FirstIsLongRunning()) ? impl.FirstResult : Config.SkippedMessage;
            var secondPuzzle = Config.ShowSecond && !(Config.SkipLongRunning && impl.SecondIsLongRunning()) ? impl.SecondResult : Config.SkippedMessage;

            var columnsToPrint = new (string Message, int Padding)[]
            {
                (string.Format(Config.DayMessageConvention, day), Config.InfoColumnPadding),
                (impl.TypeName, Config.InfoColumnPadding),
                (firstPuzzle, Config.ResultColumnPadding),
                (secondPuzzle, Config.ResultColumnPadding)
            }.Select(pair => pair.Message.PadLeft(pair.Padding));

            Console.WriteLine($"| {string.Join(" | ", columnsToPrint)} |");
        }
    }

    /** Prints the result of all days in the result table */
    public static void PrintAllSolutionMessages()
    {
        if (!Config.PrintAfterLastImpl)
        {
            var lastDay = Puzzles.PuzzleImplementationDict()
                .Last(entry => !(entry.Value.Count > 0 && entry.Value.First() == Puzzles.NoImplementation))
                .Key;
            Puzzles.PuzzleImplementationDict().Keys
                .Where(day => day <= lastDay)
                .ToList()
                .ForEach(PrintSolutionMessage);
        }
        else
        {
            Puzzles.PuzzleImplementationDict().Keys
                .ToList()
                .ForEach(PrintSolutionMessage);
        }
    }

    /** Prints the result of the last day with an implementation in the result table */
    public static void PrintLastSolutionMessage()
    {
        var lastDay = Puzzles.PuzzleImplementationDict()
            .Last(entry => entry.Value.Count > 0 && entry.Value.FirstOrDefault() != Puzzles.NoImplementation)
            .Key;
        PrintSolutionMessage(lastDay);
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
    public static void DebugPrintExcerpt<T>(IEnumerable<T> collection, string? prefix = null, int maxCount = 10, string separator = ", ")
    {
        if (Config.IsDebug)
        {
            if (prefix != null) Console.Write(prefix);
            var actuallyTaken = collection.Take(maxCount).ToList();
            var andMore = actuallyTaken.Count == maxCount ? $"... and {collection.Count() - actuallyTaken.Count} more" : string.Empty;
            Console.WriteLine($"{string.Join(separator, actuallyTaken)} {andMore}");
        }
    }
}