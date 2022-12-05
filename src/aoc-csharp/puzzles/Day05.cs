namespace aoc_csharp.puzzles;

public sealed class Day05 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var (initialStacks, amountOfStacks) = ReadInitialLoad(_input);
        var stacks = Util.InitializeListWithDefault(amountOfStacks, () => new Stack<char>());

        Printer.DebugMsg($"There are {stacks.Count} stacks.");
        PopulateWithInitial(stacks, initialStacks);
        Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");

        var moves = _input
            .Skip(initialStacks.Count + 2)
            .Select(line => line.Split(" "))
            .Select(parts => (amount: int.Parse(parts[1]), from: int.Parse(parts[3]) - 1, to: int.Parse(parts[5]) - 1))
            .ToList();

        foreach (var item in moves)
        {
            Printer.DebugMsg($"Move: {item}");
            for (int i = 0; i < item.amount; i++)
            {
                stacks[item.to].Push(stacks[item.from].Pop());
            }
            Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");
        }
        var result = string.Join("", stacks.Select(s => s.FirstOrDefault()));
        Printer.DebugMsg($"Final top crates are {result}.");
        return result;
    }

    public override string? SecondPuzzle()
    {
        var (initialStacks, amountOfStacks) = ReadInitialLoad(_input);
        var stacks = Util.InitializeListWithDefault(amountOfStacks, () => new Stack<char>());

        Printer.DebugMsg($"There are {stacks.Count} stacks.");
        PopulateWithInitial(stacks, initialStacks);
        Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");

        var moves = _input
            .Skip(initialStacks.Count + 2)
            .Select(line => line.Split(" "))
            .Select(parts => (amount: int.Parse(parts[1]), from: int.Parse(parts[3]) - 1, to: int.Parse(parts[5]) - 1))
            .ToList();

        var exchangeStack = new Stack<char>();

        foreach (var item in moves)
        {
            Printer.DebugMsg($"Move: {item}");
            for (int i = 0; i < item.amount; i++)
            {
                exchangeStack.Push(stacks[item.from].Pop());
            }
            for (int i = 0; i < item.amount; i++)
            {
                stacks[item.to].Push(exchangeStack.Pop());
            }
            Printer.DebugMsg($"Top crates are: {string.Join("", stacks.Select(s => s.LastOrDefault()))}");
        }
        var result = string.Join("", stacks.Select(s => s.FirstOrDefault()));
        Printer.DebugMsg($"Final top crates are {result}.");
        return result;
    }

    private static (List<string>, int) ReadInitialLoad(string[] input)
    {
        var initialStacks = new List<string>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            initialStacks.Add(line);
        }
        var amountOfStacks = initialStacks.Last().Split(" ").Where(split => !string.IsNullOrWhiteSpace(split)).Count();
        initialStacks.Remove(initialStacks.Last());

        Printer.DebugMsg($"There are {amountOfStacks} Stacks with initial load of:\n{string.Join("\n", initialStacks)}");
        initialStacks.Reverse();

        return (initialStacks, amountOfStacks);
    }

    private static void PopulateWithInitial (List<Stack<char>> stacks, List<string> initialInput) {
        var at = (int i) => i * 4 + 1;
        foreach (var line in initialInput)
        {
            for (var j = 0; at(j) < line.Length; j++)
            {
                Printer.DebugMsg($"Adding {line[at(j)]} to stack {j}");
                if (!char.IsWhiteSpace(line[at(j)]))
                {
                    stacks[j].Push(line[at(j)]);
                }
            }
        }
    }
}