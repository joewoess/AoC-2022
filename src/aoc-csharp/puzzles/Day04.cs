namespace aoc_csharp.puzzles;

public sealed class Day04 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var score = 0;
        var assignments = ParseCommonInput(Data);

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
        var assignments = ParseCommonInput(Data);

        foreach (var (elfA, elfB) in assignments)
        {
            Printer.DebugMsg($"Assignment is [elfA] [elfB]: [{elfA}] [{elfB}]");
            var elfARange = Util.Range(elfA.from, elfA.to).ToList();
            var elfBRange = Util.Range(elfB.from, elfB.to).ToList();

            if (elfARange.Any(elfBRange.Contains) || elfBRange.Any(elfARange.Contains))
            {
                Printer.DebugMsg($"One elf is at least a partial subset of the other");
                score++;
            }
        }

        Printer.DebugMsg($"Times assignments overlapped {score}.");
        return score.ToString();
    }

    private static List<((int from, int to) ElfA, (int from, int to) ElfB)> ParseCommonInput(string[] data)
    {
        return data
            .Select(line => line.ToStrPair())
            .Select(ass => (elfA: ass.First.ToStrPair("-"), elfB: ass.Second.ToStrPair("-")))
            .Select(elves => (elves.elfA.MapPairWith(int.Parse), elves.elfB.MapPairWith(int.Parse)))
            .ToList();
    }
}