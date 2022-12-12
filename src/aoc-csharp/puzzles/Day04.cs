namespace aoc_csharp.puzzles;

public sealed class Day04 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var score = 0;
        List<((int from, int to) ElfA, (int from, int to) ElfB)> assignments = Data
            .Select(line => line.SplitToPair())
            .Select(pairs =>  (elfA: Util.SplitToPair(pairs.First, "-"), elfB: Util.SplitToPair(pairs.Second, "-")))
            .Select(ranges => ((int.Parse(ranges.elfA.First), int.Parse(ranges.elfA.Second)), (int.Parse(ranges.elfB.First), int.Parse(ranges.elfB.Second))))
            .ToList();

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Util.Range(elfA.from, elfA.to);
            var elfBRange = Util.Range(elfB.from, elfB.to);

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
        List<((int from, int to) ElfA, (int from, int to) ElfB)> assignments = Data
            .Select(line => line.SplitToPair())
            .Select(pairs =>  (elfA: Util.SplitToPair(pairs.First, "-"), elfB: Util.SplitToPair(pairs.Second, "-")))
            .Select(ranges => ((int.Parse(ranges.elfA.First), int.Parse(ranges.elfA.Second)), (int.Parse(ranges.elfB.First), int.Parse(ranges.elfB.Second))))
            .ToList();

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Util.Range(elfA.from, elfA.to);
            var elfBRange = Util.Range(elfB.from, elfB.to);

            if (elfARange.Any(elfBRange.Contains) || elfBRange.Any(elfARange.Contains))
            {
                Printer.DebugMsg($"One elf is a subset of the other");
                score++;
            }

        }
        Printer.DebugMsg($"Times one elf was fully unneeded was {score}.");
        return score.ToString();
    }
}