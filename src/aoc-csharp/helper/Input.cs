namespace aoc_csharp;
public static class Input
{
    public static string GetInputPath()
    {
        return Config.IsDemo ? Config.InputPathDemo : Config.InputPathReal;
    }

    public static string GetInputPath(string typeName)
    {
        return Path.Combine(GetInputPath(), typeName.ToLower() + ".txt");
    }

    public static string GetInput(this IPuzzle impl)
    {
        return File.ReadAllText(GetInputPath(impl.GetType().Name));
    }

    public static string[] GetInputLines(this IPuzzle impl)
    {
        return File.ReadAllLines(GetInputPath(impl.GetType().Name));
    }
}