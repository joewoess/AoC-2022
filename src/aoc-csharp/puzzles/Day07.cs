namespace aoc_csharp.puzzles;
using System.Numerics;

public sealed class Day07 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var rootDir = buildFileTree(Data);
        DebugPrintTree(rootDir);

        var sub100kFiles = ListDirs(rootDir, new List<Dir>(), maxSizeFilter: 100_000L);
        Printer.DebugMsg($"Found {sub100kFiles.Count} dirs with size < 100k.");
        var totalSizeOfSub100kFiles = sub100kFiles.Select(b => b.Size).Aggregate(BigInteger.Add);
        return totalSizeOfSub100kFiles.ToString();
    }

    public override string? SecondPuzzle()
    {
        var rootDir = buildFileTree(Data);

        var currentTotal = rootDir.Size;
        var totalSize = 70_000_000L;
        var minSizeRequired = 30_000_000L;
        var minDirSizeToDelete = minSizeRequired - (totalSize - currentTotal);
        Printer.DebugMsg($"Current total size is {currentTotal}, min dir size to delete is {minDirSizeToDelete}.");

        var dirs = ListDirs(rootDir, new List<Dir>(), minSizeFilter: minDirSizeToDelete);
        var toDelete = dirs.MinBy(dir => dir.Size) ?? throw new Exception("No dir found to delete.");
        Printer.DebugMsg($"Deleting {toDelete.Name} with size {toDelete.Size}.");
        return toDelete.Size.ToString();
    }

    public record class Entry(string Name, Dir? Parent)
    {
        public virtual BigInteger Size => 0;
    }
    public sealed record class Dir(string Name, Dir? Parent, List<Entry> SubEntries) : Entry(Name, Parent)
    {
        public override BigInteger Size => SubEntries.Select(entry => entry.Size).Aggregate(BigInteger.Add);
    }
    public sealed record class FileEntry(string Name, Dir? Parent, BigInteger FileSize) : Entry(Name, Parent)
    {
        public override BigInteger Size => FileSize;
    }

    public static List<Dir> ListDirs(Dir currentDir, List<Dir> dirs, BigInteger? minSizeFilter = null, BigInteger? maxSizeFilter = null)
    {
        foreach (var dir in currentDir.SubEntries.OfType<Dir>())
        {
            bool isInUpperBound = !minSizeFilter.HasValue || dir.Size > minSizeFilter;
            bool isInLowerBound = !maxSizeFilter.HasValue || dir.Size < maxSizeFilter;

            if (isInLowerBound && isInUpperBound)
            {
                dirs.Add(dir);
            }

            ListDirs(dir, dirs, minSizeFilter, maxSizeFilter);
        }
        return dirs;
    }

    private static Dir buildFileTree(string[] input)
    {
        Dir rootDir = new Dir("/", null, new());
        Dir currentDir = rootDir;
        List<Entry> currentDirBuilder = new();

        foreach (var line in input)
        {
            if (line.StartsWith("$") && currentDirBuilder.Count > 0)
            {
                currentDir.SubEntries.AddRange(currentDirBuilder);
                currentDirBuilder.Clear();
            }
            switch (line.Split(" "))
            {
                case ["$", "ls"]:
                    Printer.DebugMsg($"Following are the entries for {currentDir.Name}.");
                    break;
                case ["$", "cd", "/"]:
                    currentDir = rootDir;
                    break;
                case ["$", "cd", ".."]:
                    currentDir = currentDir.Parent ?? rootDir;
                    break;
                case ["$", "cd", var name]:
                    currentDir = currentDir.SubEntries.OfType<Dir>().FirstOrDefault(d => d.Name == name) ?? throw new Exception($"Unknown dir {name}");
                    break;
                default:
                    switch (line.Split(" "))
                    {
                        case ["dir", var name]:
                            Printer.DebugMsg($"Adding dir {name} to {currentDir.Name}.");
                            currentDirBuilder.Add(new Dir(name, currentDir, new()));
                            break;
                        case [var size, var name]:
                            Printer.DebugMsg($"Adding file {name} with size {size} to {currentDir.Name}.");
                            currentDirBuilder.Add(new FileEntry(name, currentDir, BigInteger.Parse(size)));
                            break;
                    }
                    break;
            }
        }

        if (currentDirBuilder.Count > 0)
        {
            currentDir.SubEntries.AddRange(currentDirBuilder);
            currentDirBuilder.Clear();
        }
        return rootDir;
    }

    /** Prints a tree of entries with indentation */
    public static void DebugPrintTree(Entry currentEntry, int level = 0)
    {
        Printer.DebugMsg($"{new string(' ', level * 2)}+ {currentEntry.Name} ({currentEntry.Size})");
        if (currentEntry is Dir dir)
        {
            foreach (var sub in dir.SubEntries)
            {
                DebugPrintTree(sub, level + 1);
            }
        }
    }
}