namespace aoc_csharp.puzzles;

public sealed class Day04 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var score = 0;
        List<((int from, int to) ElfA, (int from, int to) ElfB)> assignments = _input
            .Select(line => SplitToPair(line))
            .Select(pairs =>  (elfA: SplitToPair(pairs.A, "-"), elfB: SplitToPair(pairs.B, "-")))
            .Select(ranges => ((int.Parse(ranges.elfA.A), int.Parse(ranges.elfA.B)), (int.Parse(ranges.elfB.A), int.Parse(ranges.elfB.B))))
            .ToList();

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Grids.Range(elfA.from, elfA.to);
            var elfBRange = Grids.Range(elfB.from, elfB.to);

            if (elfARange.All(elfBRange.Contains) || elfBRange.All(elfARange.Contains))
            {
                Printer.DebugMsg($"One elf is a full subset of the other");
                score++;
            }

        }
        Printer.DebugMsg($"Times one elf was fully unneeded was {score}.");
        return score.ToString();
    }

    public override string? SecondPuzzle()
    {
        var score = 0;
        List<((int from, int to) ElfA, (int from, int to) ElfB)> assignments = _input
            .Select(line => SplitToPair(line))
            .Select(pairs =>  (elfA: SplitToPair(pairs.A, "-"), elfB: SplitToPair(pairs.B, "-")))
            .Select(ranges => ((int.Parse(ranges.elfA.A), int.Parse(ranges.elfA.B)), (int.Parse(ranges.elfB.A), int.Parse(ranges.elfB.B))))
            .ToList();

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Grids.Range(elfA.from, elfA.to);
            var elfBRange = Grids.Range(elfB.from, elfB.to);

            if (elfARange.Any(elfBRange.Contains) || elfBRange.Any(elfARange.Contains))
            {
                Printer.DebugMsg($"One elf is a subset of the other");
                score++;
            }

        }
        Printer.DebugMsg($"Times one elf was fully unneeded was {score}.");
        return score.ToString();
    }

    private static (string A, string B) SplitToPair(string str, string seperator = ",")
    {
        var parts = str.Split(seperator);
        return (parts[0], parts[1]);
    }
}