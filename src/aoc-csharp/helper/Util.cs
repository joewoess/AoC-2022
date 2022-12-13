namespace aoc_csharp;
using System.Numerics;

public static class Util
{
    /** Initializes a list with count amount default values */
    public static List<T> InitializeListWithDefault<T>(int count, Func<T> defaultFactory)
    {
        var list = new List<T>();
        for (var i = 0; i < count; i++)
        {
            list.Add(defaultFactory.Invoke());
        }
        return list;
    }

    /** Returns an enumerable of ints between from and to counting down if needed */
    public static IEnumerable<int> Range(int from, int to)
    {
        return from > to
            ? Enumerable.Range(to, from - to + 1).Reverse()
            : Enumerable.Range(from, to - from + 1);
    }

    /** Executes the action count times */
    public static void DoTimes(this int count, Action action)
    {
        foreach (var _ in Range(1, count))
        {
            action.Invoke();
        }
    }

    /** If a string has two value split by a seperator, this parses it into a pair of 2 strings */
    public static (T First, T Second) SplitToPair<T>(this string str, Func<string, T> mapFirst, Func<string, T> mapSecond, string seperator = ",")
    {
        var parts = str.Split(seperator);
        if (parts.Length != 2)
        {
            Printer.DebugMsg($"Expected 2 parts in string '{str}' but got {parts.Length}");
        }
        return (mapFirst(parts[0]), mapSecond(parts[1]));
    }
    public static (T First, T Second) SplitToPair<T>(this string str, Func<string, T> mapBoth, string seperator = ",") => str.SplitToPair(mapBoth, mapBoth, seperator);
    public static (string First, string Second) SplitToPair(this string str, string seperator = ",") => str.SplitToPair(s => s, seperator);
    public static (T First, T Second) ApplyToPair<T>(this (T First, T Second) pair, Func<T, T> func) => (func.Invoke(pair.First), func.Invoke(pair.Second));

    /** Extension function to check if a number is between two other numbers */
    public static bool BetweenIncl<T>(this T num, T min, T max) where T : INumber<T> => (num >= min) && (num <= max);

    /** Extension function to get the string representation of a list */
    public static string ToListString<T>(this IEnumerable<T>? list, string? seperator = ",", string? prefix = "[", string? postfix = "]")
        => list != null
            ? $"{prefix}{string.Join(seperator, list)}{postfix}"
            : $"{prefix}{postfix}";
}