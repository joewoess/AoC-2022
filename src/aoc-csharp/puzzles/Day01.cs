namespace aoc_csharp.puzzles;

public sealed class Day01 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var sum = 0;
        var numbers = new List<int>();
        foreach (var line in Data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                numbers.Add(sum);
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }
        numbers.Add(sum);

        var maxCalories = numbers.Max();
        Printer.DebugMsg($"Elf with max calories had {maxCalories} total calories.");
        return maxCalories.ToString();
    }

    public override string? SecondPuzzle()
    {
        var sum = 0;
        var numbers = new List<int>();
        foreach (var line in Data)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                numbers.Add(sum);
                sum = 0;
            }
            else
            {
                sum += int.Parse(line);
            }
        }
        numbers.Add(sum);

        var maxCalories = numbers.OrderDescending().Take(3).Sum();
        Printer.DebugMsg($"Top 3 elves had {maxCalories} total calories.");
        return maxCalories.ToString();
    }
}