using static aoc_csharp.PathFinding;

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
        var fieldsVisitedByTail = new Dictionary<Point, bool>()
        {
            [startingPoint] = true
        };

        foreach (var (dir, amount) in instructions)
        {
            var amountOfSteps = Util.Range(1, amount);
            Printer.DebugMsg($"Head traveling from {headPoint} > {dir} {amount} steps.");

            foreach (var _ in amountOfSteps)
            {
                var nextHeadPoint = headPoint.StepInDirection(ParseDirection(dir));
                if (!nextHeadPoint.IsWithinReach(tailPoint))
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
        var fieldsVisitedByTail = new Dictionary<Point, bool>()
        {
            [startingPoint] = true
        };
        var rope = new Point[10];
        Array.Fill(rope, startingPoint);

        foreach (var (dir, amount) in instructions)
        {
            var amountOfSteps = Util.Range(1, amount);
            Printer.DebugMsg($"Head traveling from {rope[0]} > {dir} {amount} steps.");

            foreach (var _ in amountOfSteps)
            {
                var nextRope = rope.ToArray();
                nextRope[0] = rope[0].StepInDirection(ParseDirection(dir));
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
    private Direction ParseDirection (string dir) => dir switch
    {
        "U" => Direction.Up,
        "D" => Direction.Down,
        "L" => Direction.Left,
        "R" => Direction.Right,
        _ => throw new Exception($"Unknown direction {dir}")
    };
}