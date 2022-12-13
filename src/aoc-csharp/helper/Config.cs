namespace aoc_csharp.helper;
public static class Config
{
    // Paths

    public const string InputPathReal = "../../data/real/";
    public const string InputPathDemo = "../../data/demo/";
    public const string OutputHeaderPath = "../../HEADER";

    // Settings

    public static bool IsDebug = false;
    public static bool IsDemo = false;
    public static bool ShowLast = false;
    public static bool ShowFirst = true;
    public static bool ShowSecond = true;

    public static readonly bool TryAndUseConsoleWidth = false;
    public static readonly bool PrintAfterLastImpl = false;
    
    // Messages
    public static readonly string[] GreetingMessageLines = File.ReadAllLines(OutputHeaderPath);

    public const string NoSolutionMessage = "NO IMPL";
    public const string NoDataMessage = "NO DATA";
    public const string NoResultMessage = "NO RESULT";
    public const string SkippedMessage = "SKIPPED";

    // Constants

    public const int ResultColumnPadding = 30;
    public const int InfoColumnPadding = 15;
    public const int MaxChallengeDays = 25;
    public const char LineArtChar = '-';
    public const string ImplementationNamespace = "aoc_csharp.puzzles";
    public const string DataFileNamingConvention = "day{0:D2}.txt";
    public const string DayMessageConvention = "Day {0:D2}";
}
