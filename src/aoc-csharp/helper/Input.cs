namespace aoc_csharp;
public static class Input
{
    public static string DataDirectory => Config.IsDemo ? Config.InputPathDemo : Config.InputPathReal;

    public static string DataFilePath(string typeName) => Path.Combine(DataDirectory, typeName.ToLower() + ".txt");
    public static string DataFilePath(IPuzzle puzzle) => Path.Combine(DataDirectory, puzzle.TypeName.ToLower() + ".txt");
    public static string DataFilePath(int day) => Path.Combine(DataDirectory, string.Format(Config.DataFileNamingConvention, day));


    public static SuccessResult<string> GetInput(this IPuzzle impl)
    {
        return File.Exists(DataFilePath(impl))
            ? new(true, File.ReadAllText(DataFilePath(impl)))
            : new(false, null);
    }

    public static SuccessResult<string[]> GetInputLines(this IPuzzle impl)
    {
        return File.Exists(DataFilePath(impl))
            ? new(true, File.ReadAllLines(DataFilePath(impl)))
            : new(false, null);
    }
}