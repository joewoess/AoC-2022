namespace aoc_csharp.puzzles;

public sealed class Day04 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var score = 0;
        List<((int from, int to) ElfA, (int from, int to) ElfB)> assignments = Data
            .Select(line => line.SplitToPair())
            .Select(pairs => (elfA: pairs.First.SplitToPair("-"), elfB: pairs.Second.SplitToPair("-")))
            .Select(pairs => (pairs.elfA.ApplyToPair(int.Parse), pairs.elfB.ApplyToPair(int.Parse)))
            .ToList();

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Util.Range(elfA.from, elfA.to).ToList();
            var elfBRange = Util.Range(elfB.from, elfB.to).ToList();

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
            .Select(pairs => (elfA: pairs.First.SplitToPair("-"), elfB: pairs.Second.SplitToPair("-")))
            .Select(pairs => (pairs.elfA.ApplyToPair(int.Parse), pairs.elfB.ApplyToPair(int.Parse)))
            .ToList();

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Util.Range(elfA.from, elfA.to).ToList();
            var elfBRange = Util.Range(elfB.from, elfB.to).ToList();

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