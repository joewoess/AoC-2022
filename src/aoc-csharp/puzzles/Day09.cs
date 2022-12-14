namespace aoc_csharp.puzzles;

public sealed class Day09 : PuzzleBaseLines
{
    private static readonly Point StartingPoint = new (0, 0);
    public override string? FirstPuzzle()
    {
        var instructions = Data
            .Select(line => line.SplitAndMapToPair(s => s, int.Parse, " "))
            .ToArray();

        Printer.DebugMsg($"There are {instructions.Length} instructions.");

        var headPoint = StartingPoint;
        var tailPoint = StartingPoint;
        var fieldsVisitedByTail = new Dictionary<Point, bool>()
        {
            [StartingPoint] = true
        };

        foreach (var (dir, amount) in instructions)
        {
            var amountOfSteps = Util.Range(1, amount);
            Printer.DebugMsg($"Head traveling from {headPoint} > {dir} {amount} steps.");

            foreach (var _ in amountOfSteps)
            {
                var nextHeadPoint = headPoint.StepInDirection(dir.ParseDirection());
                if (!nextHeadPoint.IsWithinReach(tailPoint))
                {
                    tailPoint = tailPoint.StepTowards(nextHeadPoint);
                }

                headPoint = nextHeadPoint;
                fieldsVisitedByTail[tailPoint] = true;
            }
        }

        var grid = fieldsVisitedByTail.AsGrid(val => val ? "#" : ".");
        Printer.DebugMsg($"Final field:\n{grid.AsPrintable()}");
        var visitedFields = grid.AsJaggedArray().SelectMany(row => row).Count(v => v == "#");
        Printer.DebugMsg($"Found {visitedFields} fields visited.");

        return visitedFields.ToString();
    }

    public override string? SecondPuzzle()
    {
        var instructions = Data
            .Select(line => line.SplitAndMapToPair(s => s, int.Parse, " "))
            .ToArray();

        Printer.DebugMsg($"There are {instructions.Length} instructions.");

        var fieldsVisitedByTail = new Dictionary<Point, bool>()
        {
            [StartingPoint] = true
        };
        var rope = new Point[10];
        Array.Fill(rope, StartingPoint);

        foreach (var (dir, amount) in instructions)
        {
            var amountOfSteps = Util.Range(1, amount);
            Printer.DebugMsg($"Head traveling from {rope[0]} > {dir} {amount} steps.");

            foreach (var _ in amountOfSteps)
            {
                // TODO use pairwithnext
                var nextRope = rope.ToArray();
                nextRope[0] = rope[0].StepInDirection(dir.ParseDirection());
                Util.Range(1, rope.Length - 1)
                    .Where(idx => !nextRope[idx - 1].IsWithinReach(rope[idx]))
                    .ToList()
                    .ForEach(idx => nextRope[idx] = rope[idx].StepTowards(rope[idx - 1]));
                rope = nextRope;
                fieldsVisitedByTail[rope[^1]] = true;
            }

            if (Config.IsDebug)
            {
                var currentRope = new Dictionary<Point, char>();
                currentRope[StartingPoint] = 's';
                foreach (var (point, idx) in rope.Select((knot, idx) => (knot, idx)).Reverse())
                {
                    currentRope[point] = idx == 0 ? 'H' : idx.ToString()[0];
                }

                var currentLayout = currentRope.AsCharGrid();
                Printer.DebugMsg($"Layout:\n{currentLayout.AsPrintable()}");
            }
        }

        var grid = fieldsVisitedByTail.AsGrid(val => val ? "#" : ".");
        Printer.DebugMsg($"Final field:\n{grid.AsPrintable()}");
        var visitedFields = grid.AsJaggedArray().SelectMany(row => row).Count(v => v == "#");
        Printer.DebugMsg($"Found {visitedFields} fields visited.");

        return visitedFields.ToString();
    }
}