namespace aoc_csharp.puzzles;

public sealed class Day02 : PuzzleBaseLines
{
    // A = Rock, B = Paper, C = Scissors
    // Rock = 1pt, Paper = 2pt, Scissors = 3pt
    // Loss = 0pt, Draw = 3pt, Win = 6pt

    public override string? FirstPuzzle()
    {
        // X = Rock, Y = Paper, Z = Scissors
        var score = 0;
        var duels = ParseCommonInput(Data);
        foreach (var (elf, you) in duels)
        {
            var duelScore = you switch
            {
                "X" => elf switch
                {
                    "A" => 3,
                    "B" => 0,
                    "C" => 6,
                    _ => -1
                } + 1,
                "Y" => elf switch
                {
                    "A" => 6,
                    "B" => 3,
                    "C" => 0,
                    _ => -2
                } + 2,
                "Z" => elf switch
                {
                    "A" => 0,
                    "B" => 6,
                    "C" => 3,
                    _ => -3
                } + 3,
                _ => 0
            };
            if (duelScore == 0)
            {
                Printer.DebugMsg($"Invalid input. Elf chose {elf} and you chose {you}");
                return null;
            }

            Printer.DebugMsg($"Elf: {elf} vs You: {you} results in {duelScore} points.");
            score += duelScore;
        }

        Printer.DebugMsg($"Your total score is {score}.");
        return score.ToString();
    }

    public override string? SecondPuzzle()
    {
        // X = Loss, Y = Draw, Z = Win
        var score = 0;
        var duels = ParseCommonInput(Data);
        foreach (var (elf, outcome) in duels)
        {
            var duelScore = outcome switch
            {
                "X" => elf switch
                {
                    "A" => 3,
                    "B" => 1,
                    "C" => 2,
                    _ => 0
                } + 0,
                "Y" => elf switch
                {
                    "A" => 1,
                    "B" => 2,
                    "C" => 3,
                    _ => -3
                } + 3,
                "Z" => elf switch
                {
                    "A" => 2,
                    "B" => 3,
                    "C" => 1,
                    _ => -6
                } + 6,
                _ => 0
            };
            if (duelScore == 0)
            {
                Printer.DebugMsg($"Invalid input. Elf chose {elf} and you wanted {outcome}");
                return null;
            }

            Printer.DebugMsg($"Elf: {elf} with Outcome: {outcome} results in {duelScore} points.");
            score += duelScore;
        }

        Printer.DebugMsg($"Your total score is {score}.");
        return score.ToString();
    }

    private static List<(string Elf, string Desired)> ParseCommonInput(IEnumerable<string> data)
    {
        return data
            .Select(x => x.ToStrPair(" "))
            .ToList();
    }
}