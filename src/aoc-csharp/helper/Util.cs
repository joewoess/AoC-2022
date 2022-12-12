namespace aoc_csharp;

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

    public static void DoTimes(this int count, Action action)
    {
        foreach (var _ in Range(1, count))
        {
            action.Invoke();
        }
    }

    public static (string First, string Second) SplitToPair(this string str, string seperator = ",")
    {
        var parts = str.Split(seperator);
        if(parts.Length != 2)
            throw new ArgumentException($"Expected 2 parts, got {parts.Length}.");
        return (parts[0], parts[1]);
    }

    public static (string First, string Second) ApplyToPair(this (string First, string Second) pair, Func<string, string> func)
    {
        return (func.Invoke(pair.First), func.Invoke(pair.Second));
    }
}