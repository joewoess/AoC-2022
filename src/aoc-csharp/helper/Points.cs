namespace aoc_csharp;

public readonly record struct Point(int X, int Y)
{
    public override string ToString() => $"({X},{Y})";
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

public static class PointerExtensions
{
    public static Point StepInDirection(this Point currentPoint, Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Point(currentPoint.X, currentPoint.Y - 1),
            Direction.Down => new Point(currentPoint.X, currentPoint.Y + 1),
            Direction.Left => new Point(currentPoint.X - 1, currentPoint.Y),
            Direction.Right => new Point(currentPoint.X + 1, currentPoint.Y),
            Direction.UpLeft => new Point(currentPoint.X - 1, currentPoint.Y - 1),
            Direction.UpRight => new Point(currentPoint.X + 1, currentPoint.Y - 1),
            Direction.DownLeft => new Point(currentPoint.X - 1, currentPoint.Y + 1),
            Direction.DownRight => new Point(currentPoint.X + 1, currentPoint.Y + 1),
            _ => throw new Exception($"Unknown direction {dir}")
        };
    }

    public static Point StepTowards(this Point currentPoint, Point target, bool allowDiagonal = true, bool preferDiagonal = true, bool preferHorizontalOverVertical = true)
    {
        if (currentPoint == target) return currentPoint;
        var xDiff = target.X - currentPoint.X;
        var xStep = Math.Abs(xDiff) > 0 ? 1 * Math.Sign(xDiff) : 0;
        var yDiff = target.Y - currentPoint.Y;
        var yStep = Math.Abs(yDiff) > 0 ? 1 * Math.Sign(yDiff) : 0;
        // move diagonal
        if (allowDiagonal && preferDiagonal) return new Point(currentPoint.X + xStep, currentPoint.Y + yStep);
        if (allowDiagonal && !preferDiagonal && Math.Abs(currentPoint.X - target.X) == Math.Abs(currentPoint.Y - target.Y)) return new Point(currentPoint.X + xStep, currentPoint.Y + yStep);
        // move horizontal
        if (preferHorizontalOverVertical && Math.Abs(currentPoint.X - target.X) != 0) return new Point(currentPoint.X + xStep, currentPoint.Y);
        if (!preferHorizontalOverVertical && Math.Abs(currentPoint.Y - target.Y) == 0) return new Point(currentPoint.X + xStep, currentPoint.Y);
        // move vertical
        return new Point(currentPoint.X, currentPoint.Y + yStep);
    }

    public static bool IsWithinReach(this Point currentPoint, Point target)
    {
        return Math.Abs(currentPoint.X - target.X) <= 1 && Math.Abs(currentPoint.Y - target.Y) <= 1;
    }

    /** Manhattan Distance = no diagonal movement */
    public static int ManhattanDistance(this Point currentPoint, Point target)
    {
        return Math.Abs(currentPoint.X - target.X) + Math.Abs(currentPoint.Y - target.Y);
    }

    /** Chebyshev Distance = diagonal movement allowed */
    public static int ChebyshevDistance(this Point currentPoint, Point target)
    {
        return Math.Max(Math.Abs(currentPoint.X - target.X), Math.Abs(currentPoint.Y - target.Y));
    }

    /** Euclidean Distance = mathematical distance */
    public static int EuclideanDistance(this Point currentPoint, Point target)
    {
        return (int)Math.Sqrt(Math.Pow(currentPoint.X - target.X, 2) + Math.Pow(currentPoint.Y - target.Y, 2));
    }

    public static IEnumerable<Point> GetNeighborPoints(this Point currentPoint, bool allowDiagonal = true)
    {
        var directions = allowDiagonal
        ? new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right, Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight }
        : new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        return directions.Select(dir => currentPoint.StepInDirection(dir));
    }
}