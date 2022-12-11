using System.Numerics;

namespace aoc_csharp.puzzles;

public sealed class Day11 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var monkeys = _input
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Chunk(6)
            .Select(Monkey.ParseMonkeyInput)
            .ToArray();
        Printer.DebugMsg($"There are {monkeys.Length} monkeys.");

        var inspections = new Dictionary<int, int>();
        Array.ForEach(monkeys, monkey => inspections.Add(monkey.NumOfMonkey, 0));

        var numRounds = Grids.Range(1, 20);

        foreach (var _ in numRounds)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    inspections[monkey.NumOfMonkey]++;
                    var worryLevel = monkey.Items.Dequeue();
                    worryLevel = monkey.Operation(worryLevel);
                    worryLevel = Monkey.ReduceWorryLevelDefault(worryLevel);

                    monkeys[monkey.GetTargetByWorryLevel(worryLevel)].Items.Enqueue(worryLevel);
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
            .Select(Monkey.ParseMonkeyInput)
            .ToArray();

        var inspections = new Dictionary<int, int>();
        Array.ForEach(monkeys, monkey => inspections.Add(monkey.NumOfMonkey, 0));

        var numRounds = Grids.Range(1, 10_000);
        var lcd = monkeys.Select(monkey => monkey.TestDivisor).Aggregate((a, b) => a * b);


        foreach (var round in numRounds)
        {
            if (IsCheckRound(round))
            {
                Printer.DebugMsg($"== After round {round} ==");
            }
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    inspections[monkey.NumOfMonkey]++;
                    var worryLevel = monkey.Items.Dequeue();
                    worryLevel = monkey.Operation(worryLevel);
                    worryLevel = Monkey.ReduceWorryLevelByLCD(worryLevel, lcd);

                    monkeys[monkey.GetTargetByWorryLevel(worryLevel)].Items.Enqueue(worryLevel);
                }

                if (IsCheckRound(round))
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

    private sealed record Monkey(int NumOfMonkey, Queue<long> Items, Func<long, long> Operation, int TestDivisor, int monkeyNumTargetTrue, int monkeyNumTargetFalse)
    {
        public static Func<long, long> ReduceWorryLevelDefault => worryLevel => worryLevel / 3;
        public static Func<long, long, long> ReduceWorryLevelByLCD => (worryLevel, commonDivider) => worryLevel % commonDivider;
        public Func<long, bool> IsDivisible => worryLevel => worryLevel % TestDivisor == 0;
        public Func<long, int> GetTargetByWorryLevel => worryLevel => (IsDivisible(worryLevel) ? monkeyNumTargetTrue : monkeyNumTargetFalse);

        public static Monkey ParseMonkeyInput(string[] dataChunk)
        {
            return new Monkey(
                        NumOfMonkey: int.Parse(dataChunk[0].Split(" ").Last().TrimEnd(':')),
                        Items: new Queue<long>(dataChunk[1].Substring(dataChunk[1].IndexOf(':') + 2).Split(", ").Select(long.Parse)),
                        Operation: (dataChunk[2].Substring(dataChunk[2].IndexOf('=') + 2).Trim().Split(" ")) switch
                        {
                            ["old", "+", "old"] => (val => val + val),
                            ["old", "*", "old"] => (val => val * val),
                            ["old", "+", var num] => (val => val + int.Parse(num)),
                            ["old", "*", var num] => (val => val * int.Parse(num)),
                            _ => (val => val)
                        },
                        TestDivisor: int.Parse(dataChunk[3].Split(" ").Last()),
                        monkeyNumTargetTrue: int.Parse(dataChunk[4].Split(" ").Last()),
                        monkeyNumTargetFalse: int.Parse(dataChunk[5].Split(" ").Last()));
        }
    }

    private bool IsCheckRound(int round) => round == 1 || round == 20 || round % 1000 == 0;
}