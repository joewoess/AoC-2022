using System.Numerics;

namespace aoc_csharp.puzzles;

public sealed class Day11 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var monkeys = _input
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Chunk(6)
            .Select(ParseMonkeyInput)
            .ToArray();

        Printer.DebugMsg($"There are {monkeys.Length} monkeys.");

        var inspections = new Dictionary<ushort, uint>();
        Array.ForEach(monkeys, monkey => inspections.Add(monkey.NumOfMonkey, 0));

        var targetRounds = 20;
        var numRounds = Grids.Range(1, targetRounds).ToArray();

        foreach (var _ in numRounds)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.StartingItems.Count > 0)
                {
                    inspections[monkey.NumOfMonkey]++;
                    var worryLevel = monkey.StartingItems.Dequeue();
                    worryLevel = monkey.Operation(worryLevel);
                    worryLevel = Monkey.ReducedWorryLevel(worryLevel);

                    monkeys[monkey.GetTargetByWorryLevel(worryLevel)].StartingItems.Enqueue(worryLevel);
                }
            }
        }

        var mostActiveMonkeys = inspections.OrderByDescending(kvp => kvp.Value).Take(2).ToArray();
        Printer.DebugMsg($"Two most active monkeys are [Monkey, Inspections]: [{mostActiveMonkeys[0].Key}, {mostActiveMonkeys[0].Value}] and [{mostActiveMonkeys[1].Key}, {mostActiveMonkeys[1].Value}].");
        var product = mostActiveMonkeys[0].Value * mostActiveMonkeys[1].Value;
        Printer.DebugMsg($"The product of their numbers is {product}.");
        return product.ToString();
    }

    public override string? SecondPuzzle()
    {
        var monkeys = _input
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Chunk(6)
            .Select(ParseMonkeyInput)
            .ToArray();

        var inspections = new Dictionary<ushort, uint>();
        Array.ForEach(monkeys, monkey => inspections.Add(monkey.NumOfMonkey, 0));

        var minDivider = monkeys.Select(m => m.TestDivisor).Aggregate((a, b) => a * b);

        const int targetRounds = 10_000;

        for (var round = 1; round <= targetRounds; round++)
        {
            if (round == 1 || round == 20 || round % 1000 == 0)
            {
                Printer.DebugMsg($"== After round {round} ==");
            }
            foreach (var monkey in monkeys)
            {
                while (monkey.StartingItems.Count > 0)
                {
                    inspections[monkey.NumOfMonkey]++;
                    var worryLevel = monkey.StartingItems.Dequeue();
                    worryLevel = monkey.Operation(worryLevel);
                    if(worryLevel % minDivider == 0)
                    {
                        worryLevel = worryLevel / minDivider;
                    }

                    monkeys[monkey.GetTargetByWorryLevel(worryLevel)].StartingItems.Enqueue(worryLevel);
                }
                if (round == 1 || round == 20 || round % 1000 == 0)
                {
                    Printer.DebugMsg($"  Monkey {monkey.NumOfMonkey} inspected items {inspections[monkey.NumOfMonkey]} times.");
                }
            }
        }

        var mostActiveMonkeys = inspections.OrderByDescending(kvp => kvp.Value).Take(2).ToArray();
        Printer.DebugMsg($"Two most active monkeys are [Monkey, Inspections]: [{mostActiveMonkeys[0].Key}, {mostActiveMonkeys[0].Value}] and [{mostActiveMonkeys[1].Key}, {mostActiveMonkeys[1].Value}].");
        var product = BigInteger.Multiply(mostActiveMonkeys[0].Value, mostActiveMonkeys[1].Value);
        Printer.DebugMsg($"The product of their numbers is {product}.");
        return product.ToString();
    }

    private sealed record Monkey(ushort NumOfMonkey, Queue<BigInteger> StartingItems, Func<BigInteger, BigInteger> Operation, uint TestDivisor, ushort monkeyNumTargetTrue, ushort monkeyNumTargetFalse)
    {
        public static Func<BigInteger, BigInteger> ReducedWorryLevel => s => s / 3;
        public Func<BigInteger, bool> IsDivisible => worryLevel => worryLevel % TestDivisor == 0;
        public Func<BigInteger, ushort> GetTargetByWorryLevel => worryLevel => (IsDivisible(worryLevel) ? monkeyNumTargetTrue : monkeyNumTargetFalse);
    }

    private Monkey ParseMonkeyInput(string[] dataChunk)
    {
        return new Monkey(
                    NumOfMonkey: ushort.Parse(dataChunk[0].Split(" ").Last().TrimEnd(':')),
                    StartingItems: new Queue<BigInteger> (dataChunk[1].Substring(dataChunk[1].IndexOf(':') + 2).Split(", ").Select(BigInteger.Parse)),
                    Operation: (dataChunk[2].Substring(dataChunk[2].IndexOf('=') + 2).Trim().Split(" ")) switch
                    {
                        ["old", "+", "old"] => (val => val + val),
                        ["old", "*", "old"] => (val => val * val),
                        ["old", "+", var num] => (val => val + uint.Parse(num)),
                        ["old", "*", var num] => (val => val * uint.Parse(num)),
                        _ => (val => val)
                    },
                    TestDivisor: ushort.Parse(dataChunk[3].Split(" ").Last()),
                    monkeyNumTargetTrue: ushort.Parse(dataChunk[4].Split(" ").Last()),
                    monkeyNumTargetFalse: ushort.Parse(dataChunk[5].Split(" ").Last()));
    }
}