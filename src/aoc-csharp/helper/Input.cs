namespace aoc_csharp.helper;

public static class Input
{
    public static string DataDirectory => Config.IsDemo ? Config.InputPathDemo : Config.InputPathReal;
    public static string DataFilePath(IPuzzle puzzle) => Path.Combine(DataDirectory, $"{puzzle.TypeName.ToLower()}.txt");

    public static SuccessResult<string> GetInput(this IPuzzle impl)
    {
        return File.Exists(DataFilePath(impl))
            ? new SuccessResult<string>(true, File.ReadAllText(DataFilePath(impl)))
            : new SuccessResult<string>(false, null);
    }

    public static SuccessResult<string[]> GetInputLines(this IPuzzle impl)
    {
        return File.Exists(DataFilePath(impl))
            ? new SuccessResult<string[]>(true, File.ReadAllLines(DataFilePath(impl)))
            : new SuccessResult<string[]>(false, null);
    }
}