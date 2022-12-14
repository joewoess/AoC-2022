namespace aoc_csharp.puzzles;

public sealed class Day05 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var (initialStacks, amountOfStacks) = ReadInitialLoad(Data);
        var stacks = Util.InitializeListWithDefault(amountOfStacks, () => new Stack<char>());

        Printer.DebugMsg($"There are {stacks.Count} stacks.");
        PopulateWithInitial(stacks, initialStacks);
        Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");

        var moves = ParseMoves(initialStacks.Count);

        foreach (var (amount, from, to) in moves)
        {
            Printer.DebugMsg($"Move: ({amount} from {from} to {to})");
            amount.DoTimes(() => stacks[to].Push(stacks[from].Pop()));

            Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");
        }

        var result = string.Join("", stacks.Select(s => s.FirstOrDefault()));
        Printer.DebugMsg($"Final top crates are {result}.");
        return result;
    }

    public override string? SecondPuzzle()
    {
        var (initialStacks, amountOfStacks) = ReadInitialLoad(Data);
        var stacks = Util.InitializeListWithDefault(amountOfStacks, () => new Stack<char>());

        Printer.DebugMsg($"There are {stacks.Count} stacks.");
        PopulateWithInitial(stacks, initialStacks);
        Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");

        var moves = ParseMoves(initialStacks.Count);

        var exchangeStack = new Stack<char>();
        foreach (var (amount, from, to) in moves)
        {
            Printer.DebugMsg($"Move: ({amount} from {from} to {to})");
            
            amount.DoTimes(() => exchangeStack.Push(stacks[from].Pop()));
            amount.DoTimes(() => stacks[to].Push(exchangeStack.Pop()));

            Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");
        }

        var result = string.Join("", stacks.Select(s => s.FirstOrDefault()));
        Printer.DebugMsg($"Final top crates are {result}.");
        return result;
    }

    private static (List<string> Initial, int Amount) ReadInitialLoad(string[] data)
    {
        var initialStacks = data.TakeWhile(Util.HasContent).ToList();

        var amountOfStacks = initialStacks.Last().Split(" ").Count(Util.HasContent);
        initialStacks.Remove(initialStacks.Last());

        Printer.DebugMsg($"There are {amountOfStacks} Stacks with initial load of:\n{string.Join("\n", initialStacks)}");
        initialStacks.Reverse();

        return (initialStacks, amountOfStacks);
    }

    private static void PopulateWithInitial(List<Stack<char>> stacks, List<string> initialInput)
    {
        int IdxOf(int i) => i * 4 + 1;
        foreach (var line in initialInput)
        {
            for (var num = 0; IdxOf(num) < line.Length; num++)
            {
                Printer.DebugMsg($"Adding {line[IdxOf(num)]} to stack {num}");
                if (!char.IsWhiteSpace(line[IdxOf(num)]))
                {
                    stacks[num].Push(line[IdxOf(num)]);
                }
            }
        }
    }

    private List<(int Amount, int From, int To)> ParseMoves(int numInitialStacks)
    {
        return Data
            .Skip(numInitialStacks + 2)
            .Select(line => line.Split(" "))
            .Select(parts => (int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1))
            .ToList();
    }
}