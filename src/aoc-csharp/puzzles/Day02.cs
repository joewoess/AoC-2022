namespace aoc_csharp.puzzles;

public sealed class Day02 : PuzzleBaseLines
{
    /**
      * A = Rock
      * B = Paper
      * C = Scissors
  
      * Rock = 1pt
      * Paper = 2pt
      * Scissors = 3pt
  
      * Loss = 0pt
      * Draw = 3pt
      * Win = 6pt
    */

    public override string? FirstPuzzle()
    {
        /**
          * X = Rock
          * Y = Paper
          * Z = Scissors
        */
        var score = 0;
        List<(string Elf, string You)> duels = _input.Select(x => x.Split(" ").ToArray()).Select(x => (x[0], x[1])).ToList();
        foreach (var duel in duels)
        {
            var duelScore = duel.You switch
            {
                "X" => duel.Elf switch
                {
                    "A" => 3,
                    "B" => 0,
                    "C" => 6,
                    _ => -1
                } + 1,
                "Y" => duel.Elf switch
                {
                    "A" => 6,
                    "B" => 3,
                    "C" => 0,
                    _ => -2
                } + 2,
                "Z" => duel.Elf switch
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
                Printer.DebugMsg($"Invalid input. Elf chose {duel.Elf} and you chose {duel.You}");
                return null;
            }
            Printer.DebugMsg($"Elf: {duel.Elf} vs You: {duel.You} results in {duelScore} points.");
            score += duelScore;
        }
        Printer.DebugMsg($"Your total score is {score}.");
        return score.ToString();
    }

    public override string? SecondPuzzle()
    {
        /**
          * X = Loss
          * Y = Draw
          * Z = Win
        */
        var score = 0;
        List<(string Elf, string Outcome)> duels = _input.Select(x => x.Split(" ").ToArray()).Select(x => (x[0], x[1])).ToList();
        foreach (var duel in duels)
        {
            var duelScore = duel.Outcome switch
            {
                "X" => duel.Elf switch
                {
                    "A" => 3,
                    "B" => 1,
                    "C" => 2,
                    _ => 0
                } + 0,
                "Y" => duel.Elf switch
                {
                    "A" => 1,
                    "B" => 2,
                    "C" => 3,
                    _ => -3
                } + 3,
                "Z" => duel.Elf switch
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
                Printer.DebugMsg($"Invalid input. Elf chose {duel.Elf} and you wanted {duel.Outcome}");
                return null;
            }
            Printer.DebugMsg($"Elf: {duel.Elf} with Outcome: {duel.Outcome} results in {duelScore} points.");
            score += duelScore;
        }
        Printer.DebugMsg($"Your total score is {score}.");
        return score.ToString();
    }
}