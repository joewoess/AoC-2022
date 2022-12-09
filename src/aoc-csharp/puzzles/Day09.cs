namespace aoc_csharp.puzzles;

public sealed class Day09 : PuzzleBaseLines
{

    public override string? FirstPuzzle()
    {
        var instructions = _input
            .Select(line => line.Split(" ").ToArray())
            .Select(line => (dir: line[0], amount: int.Parse(line[1])))
            .ToArray();

        Printer.DebugMsg($"There are {instructions.Length} instructions.");

        var startingPoint = new Point(0, 0);
        var headPoint = startingPoint;
        var tailPoint = startingPoint;
        var fieldsVisitedByTail = new Dictionary<Point, bool>() {
            [startingPoint] = true
        };

        foreach (var (dir, amount) in instructions)
        {
            var amountOfSteps = Grids.Range(1, amount);
            Printer.DebugMsg($"Head traveling from {headPoint} > {dir} {amount} steps.");

            foreach (var _ in amountOfSteps)
            {
                var nextHeadPoint = headPoint.StepInDirection(dir);
                if (!nextHeadPoint.IsNeighborPoint(tailPoint))
                {
                    tailPoint = tailPoint.StepTowards(nextHeadPoint);
                }
                headPoint = nextHeadPoint;
                fieldsVisitedByTail[tailPoint] = true;
            }
        }

        var grid = Grids.PointDictToGrid(fieldsVisitedByTail, val => val ? "#" : ".");
        Printer.DebugMsg($"Final field:\n{Grids.GridAsPrintable(grid)}");
        var visitedFields = Grids.PointDictToGrid(fieldsVisitedByTail, val => val ? "#" : ".").ToJaggedArray().SelectMany(row => row).Count(v => v == "#");
        Printer.DebugMsg($"Found {visitedFields} fields visited.");

        return visitedFields.ToString();
    }

    public override string? SecondPuzzle()
    {
        var instructions = _input
            .Select(line => line.Split(" ").ToArray())
            .Select(line => (dir: line[0], amount: int.Parse(line[1])))
            .ToArray();

        Printer.DebugMsg($"There are {instructions.Length} instructions.");

        var startingPoint = new Point(0, 0);
        var fieldsVisitedByTail = new Dictionary<Point, bool>() {
            [startingPoint] = true
        };
        var rope = new Point[10];
        Array.Fill(rope, startingPoint);

        foreach (var (dir, amount) in instructions)
        {
            var amountOfSteps = Grids.Range(1, amount);
            Printer.DebugMsg($"Head traveling from {rope[0]} > {dir} {amount} steps.");

            foreach (var _ in amountOfSteps)
            {
                var nextRope = rope.ToArray();
                nextRope[0] = rope[0].StepInDirection(dir);
                for (var idx = 1; idx < rope.Length; idx++)
                {
                    if (!nextRope[idx - 1].IsNeighborPoint(rope[idx]))
                    {
                        nextRope[idx] = rope[idx].StepTowards(nextRope[idx - 1]);
                    }
                }
                rope = nextRope;
                fieldsVisitedByTail[rope[^1]] = true;
            }
            if(Config.IsDebug) {
                var currentRope = new Dictionary<Point, char>();
                currentRope[startingPoint] = 's';
                foreach (var (point, idx) in rope.Select((knot, idx) => (knot, idx)).Reverse())
                {
                    currentRope[point] = idx == 0 ? 'H' : idx.ToString()[0];
                }
                var currentLayout = Grids.PointDictToGrid(currentRope, val => val == default ? "." : val.ToString());
                Printer.DebugMsg($"Layout:\n{Grids.GridAsPrintable(currentLayout, val => val)}");
            }
        }

        var grid = Grids.PointDictToGrid(fieldsVisitedByTail, val => val ? "#" : ".");
        Printer.DebugMsg($"Final field:\n{Grids.GridAsPrintable(grid)}");
        var visitedFields = Grids.PointDictToGrid(fieldsVisitedByTail, val => val ? "#" : ".").ToJaggedArray().SelectMany(row => row).Count(v => v == "#");
        Printer.DebugMsg($"Found {visitedFields} fields visited.");

        return visitedFields.ToString();
    }
}

public static class PointerExtensions
{
    public static Point StepInDirection(this Point currentPoint, string dir)
    {
        switch (dir)
        {
            case "U":
                return new Point(currentPoint.X, currentPoint.Y - 1);
            case "D":
                return new Point(currentPoint.X, currentPoint.Y + 1);
            case "L":
                return new Point(currentPoint.X - 1, currentPoint.Y);
            case "R":
                return new Point(currentPoint.X + 1, currentPoint.Y);
            default: throw new Exception($"Unknown direction {dir}");
        }
    }

    public static Point StepTowards(this Point currentPoint, Point target)
    {
        var xDiff = target.X - currentPoint.X;
        var xStep = Math.Abs(xDiff) > 0 ? 1 * Math.Sign(xDiff) : 0;
        var yDiff = target.Y - currentPoint.Y;
        var yStep = Math.Abs(yDiff) > 0 ? 1 * Math.Sign(yDiff) : 0;
        return new Point(currentPoint.X + xStep, currentPoint.Y + yStep);
    }

    public static bool IsNeighborPoint(this Point currentPoint, Point target)
    {
        return Math.Abs(currentPoint.X - target.X) <= 1 && Math.Abs(currentPoint.Y - target.Y) <= 1;
    }
}