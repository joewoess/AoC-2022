namespace aoc_csharp;

public static class Util {
    public static List<T> InitializeListWithDefault<T>(int count, Func<T> defaultFactory) {
        var list = new List<T>();
        for (var i = 0; i < count; i++) {
            list.Add(defaultFactory.Invoke());
        }
        return list;
    }
}