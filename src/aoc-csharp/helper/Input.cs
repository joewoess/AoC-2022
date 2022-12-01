namespace aoc_csharp;
public static class Input
{
    public static string GetInputPath()
    {
        return Config.IsDemo ? Config.InputPathDemo : Config.InputPathReal;
    }

    public static string GetInputPath(string day)
    {
        return $"{GetInputPath()}Day {day}.txt";
    }

    public static string GetInput(string day)
    {
        return File.ReadAllText(GetInputPath(day));
    }

    public static string[] GetInputLines(string day)
    {
        return File.ReadAllLines(GetInputPath(day));
    }
}