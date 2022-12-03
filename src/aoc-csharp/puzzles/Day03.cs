namespace aoc_csharp.puzzles;

public class Day03 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var score = 0;
        // Compartments A and B are split in the middle of a line
        List<(string A, string B)> rucksacks = _input
            .Select(x => (x.Substring(0, x.Length / 2), x.Substring(x.Length / 2)))
            .ToList();

        foreach (var rucksack in rucksacks)
        {
            Printer.DebugMsg($"Rucksack Compartments [A] [B]: [{rucksack.A}] [{rucksack.B}]");
            var commonChars = rucksack.A
                .Intersect(rucksack.B)
                .ToList();
            Printer.DebugMsg($"Common Characters: [{string.Join(", ", commonChars)}]");
            if (commonChars.Count > 1)
            {
                Printer.DebugMsg($"Too many common characters, cancelling...");
                return null;
            }
            var commonCharValue = GetCommonCharValue(commonChars.First());
            Printer.DebugMsg($"Common Character Value: [{commonCharValue}]");
            score += commonCharValue;
        }
        Printer.DebugMsg($"Your total score is {score}.");
        return score.ToString();
    }

    public override string? SecondPuzzle()
    {
        var score = 0;
        // Groups of 3 lines have a common badge 
        var rucksackGroups = _input.Chunk(3).ToList();
        foreach (var group in rucksackGroups)
        {
            var commonChars = group[0]
                .Intersect(group[1])
                .Intersect(group[2])
                .ToList();
            Printer.DebugMsg($"Common Characters: [{string.Join(", ", commonChars)}]");
            if (commonChars.Count > 1)
            {
                Printer.DebugMsg($"Too many common characters, cancelling...");
                return null;
            }
            var commonCharValue = GetCommonCharValue(commonChars.First());
            Printer.DebugMsg($"Common Character Value: [{commonCharValue}]");
            score += commonCharValue;
        }
        Printer.DebugMsg($"Your total score is {score}.");
        return score.ToString();
    }

    private static int GetCommonCharValue(char commonChar)
    {
        return char.IsUpper(commonChar)
            ? commonChar - (int)'A' + 27
            : commonChar - (int)'a' + 1;
    }
}