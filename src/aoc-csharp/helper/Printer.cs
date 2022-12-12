namespace aoc_csharp;
public static class Printer
{
    /** Prints the beginning header of console output */
    public static void PrintGreeting()
    {
        PrintSeparator();
        Console.WriteLine(Config.GreetingMessage);
        PrintSeparator();
    }

    /** Prints a separator line spanning the console width */
    public static void PrintSolutionMessage(Type targetType)
    {
        var result = Puzzles.GetPuzzle(targetType);
        Console.WriteLine(
            $"| {targetType.Name} |  {(result?.FirstPuzzle() ?? Config.NoSolutionMessage).PadLeft(Config.SolutionPadding)} |  {(result?.SecondPuzzle() ?? Config.NoSolutionMessage).PadLeft(Config.SolutionPadding)} |");
    }

    /** Prints a header for the results table */
    public static void PrintResultHeader()
    {
        Console.WriteLine($"|  Day  |         1st |         2nd |");
    }

    /** Prints a separator line spanning the console width */
    public static void PrintSeparator()
    {
        Console.WriteLine(new string(Config.SeparatorChar, Console.WindowWidth));
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