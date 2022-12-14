using System.Numerics;

namespace aoc_csharp.puzzles;

public sealed class Day11 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var monkeys = ParseCommonInput(Data);
        Printer.DebugMsg($"There are {monkeys.Length} monkeys.");

        var inspections = new Dictionary<int, int>();
        Array.ForEach(monkeys, monkey => inspections.Add(monkey.NumOfMonkey, 0));

        var numRounds = Util.Range(1, 20);

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

        var (first, second) = MostActiveMonkeys(inspections);
        Printer.DebugMsg($"Two most active monkeys are [Monkey, Inspections]: [{first.Key}, {first.Value}] and [{second.Key}, {second.Value}].");
        var product = BigInteger.Multiply(first.Value, second.Value);
        Printer.DebugMsg($"The product of their numbers is {product}.");
        return product.ToString();
    }

    public override string? SecondPuzzle()
    {
        var monkeys = ParseCommonInput(Data);

        var inspections = new Dictionary<int, int>();
        Array.ForEach(monkeys, monkey => inspections.Add(monkey.NumOfMonkey, 0));

        var numRounds = Util.Range(1, 10_000);
        var lcd = monkeys.Select(monkey => monkey.TestDivisor).Aggregate((a, b) => (a * b));

        foreach (var round in numRounds)
        {
            if (IsCheckRound(round)) Printer.DebugMsg($"== After round {round} ==");

            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    inspections[monkey.NumOfMonkey]++;
                    var worryLevel = monkey.Items.Dequeue();
                    worryLevel = monkey.Operation(worryLevel);
                    worryLevel = Monkey.ReduceWorryLevelByLcd(worryLevel, lcd);
                    monkeys[monkey.GetTargetByWorryLevel(worryLevel)].Items.Enqueue(worryLevel);
                }

                if (IsCheckRound(round)) Printer.DebugMsg($"  Monkey {monkey.NumOfMonkey} inspected items {inspections[monkey.NumOfMonkey]} times.");
            }
        }

        var (first, second) = MostActiveMonkeys(inspections);
        Printer.DebugMsg($"Two most active monkeys are [Monkey, Inspections]: [{first.Key}, {first.Value}] and [{second.Key}, {second.Value}].");
        var product = BigInteger.Multiply(first.Value, second.Value);
        Printer.DebugMsg($"The product of their numbers is {product}.");
        return product.ToString();
    }

    private sealed record Monkey(int NumOfMonkey, Queue<long> Items, Func<long, long> Operation, int TestDivisor, int MonkeyNumTargetTrue, int MonkeyNumTargetFalse)
    {
        public static Func<long, long> ReduceWorryLevelDefault => worryLevel => worryLevel / 3;
        public static Func<long, long, long> ReduceWorryLevelByLcd => (worryLevel, commonDivider) => worryLevel % commonDivider;
        public Func<long, bool> IsDivisible => worryLevel => worryLevel % TestDivisor == 0;
        public Func<long, int> GetTargetByWorryLevel => worryLevel => (IsDivisible(worryLevel) ? MonkeyNumTargetTrue : MonkeyNumTargetFalse);

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
                MonkeyNumTargetTrue: int.Parse(dataChunk[4].Split(" ").Last()),
                MonkeyNumTargetFalse: int.Parse(dataChunk[5].Split(" ").Last()));
        }
    }

    private static Monkey[] ParseCommonInput(string[] data)
    {
        return data
            .Where(Util.HasContent)
            .Chunk(6)
            .Select(Monkey.ParseMonkeyInput)
            .ToArray();
    }

    private static (KeyValuePair<int, int> First, KeyValuePair<int, int> Second) MostActiveMonkeys(Dictionary<int, int> inspections)
    {
        var mostActiveMonkeys = inspections.OrderByDescending(kvp => kvp.Value).Take(2).ToArray();
        return (mostActiveMonkeys[0], mostActiveMonkeys[1]);
    }

    private static bool IsCheckRound(int round) => round is 1 or 20 || round % 1000 == 0;
}