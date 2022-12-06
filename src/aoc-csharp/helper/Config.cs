namespace aoc_csharp;
public static class Config
{
    //Paths

    public const string InputPathReal = "../../data/real/";
    public const string InputPathDemo = "../../data/demo/";

    //Settings

    public static bool IsDebug = false;
    public static bool IsDemo = false;


    //Messages

    public const string GreetingMessage =
@"AdventOfCode Runner for 2022
Challenge at: https://adventofcode.com/2022/
Author: Johannes Wöß
Written in C# 11 / .NET 7";

    public const string NoSolutionMessage = "NONE";

    //Constants

    public const int SolutionPadding = 10;
    public const int MaxChallengeDays = 25;
    public const char SeparatorChar = '-';
    public const string ImplementationNamespace = "aoc_csharp.puzzles";

}
