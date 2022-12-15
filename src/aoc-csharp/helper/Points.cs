namespace aoc_csharp.helper;

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
    // stepping

    /** Returns the next step in a given direction */
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

    /** Returns the next step between points with parameters concerning diagonals */
    public static Point StepTowards(this Point currentPoint, Point target, bool allowDiagonal = true, bool preferDiagonal = true, bool preferHorizontalOverVertical = true)
    {
        if (currentPoint == target) return currentPoint;
        var (xStep, yStep) = currentPoint.CalculateStepTowards(target);
        return (allowDiagonal, preferDiagonal, preferHorizontalOverVertical) switch
        {
            // move diagonal
            (true, true, _) => new Point(currentPoint.X + xStep, currentPoint.Y + yStep),
            (true, false, _) when Math.Abs(currentPoint.X - target.X) == Math.Abs(currentPoint.Y - target.Y) => new Point(currentPoint.X + xStep, currentPoint.Y + yStep),
            // move horizontal
            (_, _, true) when Math.Abs(currentPoint.X - target.X) != 0 => new Point(currentPoint.X + xStep, currentPoint.Y),
            (_, _, false) when Math.Abs(currentPoint.Y - target.Y) == 0 => new Point(currentPoint.X + xStep, currentPoint.Y),
            // move vertical
            _ => new Point(currentPoint.X, currentPoint.Y + yStep),
        };
    }

    /** Returns a char representation for the next step between points */
    public static char GetDirectionCharTowards(this Point currentPoint, Point target)
    {
        return currentPoint.GetDirectionTowards(target).ParseCharFromDirection();
    }

    /** Parse char representation from a direction  */
    public static char ParseCharFromDirection(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => '^',
            Direction.Down => 'v',
            Direction.Left => '<',
            Direction.Right => '>',
            Direction.UpLeft => '↖',
            Direction.UpRight => '↗',
            Direction.DownLeft => '↙',
            Direction.DownRight => '↘',
            _ => throw new Exception($"Unknown direction {direction}")
        };
    }

    /** Returns direction to next step as a point clamped to (-1, 1) */
    private static Point CalculateStepTowards(this Point currentPoint, Point target)
    {
        var (xDiff, yDiff) = (target.X - currentPoint.X, target.Y - currentPoint.Y);
        return new Point(Math.Clamp(xDiff, -1, 1), Math.Clamp(yDiff, -1, 1));
    }

    /** Returns an enumerable of points between from and to */
    public static IEnumerable<Point> WalkDirectlyTowards(this Point from, Point to, bool allowDiagonal = true, bool preferDiagonal = true,
        bool preferHorizontal = true, bool includeStart = false)
    {
        var currentPos = from;
        if (includeStart) yield return currentPos;
        while (currentPos != to)
        {
            currentPos = currentPos.StepTowards(to, allowDiagonal, preferDiagonal, preferHorizontal);
            yield return currentPos;
        }
    }

    // comparisons

    public static bool IsWithinReach(this Point currentPoint, Point target) => currentPoint.ChebyshevDistance(target) <= 1;

    /** Gets Direction to another point. This prefers diagonals */
    public static Direction GetDirectionTowards(this Point currentPoint, Point target)
    {
        if (currentPoint == target) return Direction.Up;
        return currentPoint.CalculateStepTowards(target) switch
        {
            (1, 0) => Direction.Right,
            (-1, 0) => Direction.Left,
            (0, 1) => Direction.Down,
            (0, -1) => Direction.Up,
            (1, 1) => Direction.DownRight,
            (1, -1) => Direction.UpRight,
            (-1, 1) => Direction.DownLeft,
            (-1, -1) => Direction.UpLeft,
            _ => throw new Exception($"Unknown direction towards {target} from {currentPoint}")
        };
    }

    /** Parse direction from a char representation */
    public static Direction ParseDirection(this string direction)
    {
        return direction switch
        {
            "U" or "^" => Direction.Up,
            "D" or "v" => Direction.Down,
            "L" or "<" => Direction.Left,
            "R" or ">" => Direction.Right,
            "UL" or "↖" => Direction.UpLeft,
            "UR" or "↗" => Direction.UpRight,
            "DL" or "↙" => Direction.DownLeft,
            "DR" or "↘" => Direction.DownRight,
            _ => throw new Exception($"Unknown direction {direction}")
        };
    }

    /** Returns the points neighbors of a given point */
    public static IEnumerable<Point> GetNeighborPoints(this Point currentPoint, bool allowDiagonal = true)
    {
        var directions = allowDiagonal
            ? new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right, Direction.UpLeft, Direction.UpRight, Direction.DownLeft, Direction.DownRight }
            : new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        return directions.Select(dir => currentPoint.StepInDirection(dir));
    }

    // distances

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
}