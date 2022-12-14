namespace aoc_csharp.helper;

public static class Input
{
    private static string DataDirectory => Config.IsDemo ? Config.InputPathDemo : Config.InputPathReal;
    
    private static string DataFileName(IPuzzle puzzle) => $"{puzzle.TypeName.ToLower()}.txt";
    private static string DataFilePath(IPuzzle puzzle) => Path.Combine(DataDirectory, DataFileName(puzzle));

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