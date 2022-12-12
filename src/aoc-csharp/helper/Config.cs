namespace aoc_csharp;
public static class Config
{
    // Paths

    public const string InputPathReal = "../../data/real/";
    public const string InputPathDemo = "../../data/demo/";

    // Settings

    public static bool IsDebug = false;
    public static bool IsDemo = false;
    public static bool ShowLast = false;
    public static bool ShowFirst = true;
    public static bool ShowSecond = true;

    public static bool TryAndUseConsoleWidth = false;
    public static bool PrintAfterLastImpl = false;


    // Messages

    public static string[] GreetingMessageLines = new string[] {
            "AdventOfCode Runner for 2022",
            "Challenge at: https://adventofcode.com/2022/",
            "Author: Johannes Wöß",
            "Written in C# 11 / .NET 7"
        };

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
