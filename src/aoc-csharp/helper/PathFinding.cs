namespace aoc_csharp;
public static class PathFinding
{
    /** Returns an enumerable of points between from and to going preferring to go horizontal then diagonal then vertical */
    public static IEnumerable<Point> WalkNoObstacles(Point from, Point to, bool allowDiagonal = true, bool preferDiagonal = true, bool preferHorizontal = true)
    {
        var currentPos = from;
        while (currentPos != to)
        {
            currentPos = currentPos.StepTowards(to, allowDiagonal, preferDiagonal, preferHorizontal);
            yield return currentPos;
        }
    }

    public static List<TGrid> MapNeighbors<TGrid>(this Point currentPos, Func<Point, Point, TGrid> mapper, Func<Point, Point, bool> filter, int maxHeight, int maxWidth, bool includeDiagonals = false)
    {
        return currentPos
            .GetNeighborsFiltered(filter, maxHeight, maxWidth, includeDiagonals)
            .Select(pos => mapper(currentPos, pos))
            .ToList();
    }

    public static List<Point> GetNeighborsFiltered(this Point currentPos, Func<Point, Point, bool> filter, int maxHeight, int maxWidth, bool includeDiagonals = false)
    {
        return currentPos.GetNeighborPoints(includeDiagonals)
            .Where(pos => pos.X >= 0 && pos.X < maxWidth)
            .Where(pos => pos.Y >= 0 && pos.Y < maxHeight)
            .Where(pos => filter(currentPos, pos))
            .ToList();
    }

    public static IEnumerable<TGrid> GetNeighbors<TGrid>(TGrid[][] grid, int currentY, int currentX, bool includeDiagonals = false)
    {
        if (currentY > 0) yield return grid[currentY - 1][currentX];
        if (currentY < grid.Length - 1) yield return grid[currentY + 1][currentX];
        if (currentX > 0) yield return grid[currentY][currentX - 1];
        if (currentX < grid[currentY].Length - 1) yield return grid[currentY][currentX + 1];

        if (includeDiagonals)
        {
            if (currentY > 0 && currentX > 0) yield return grid[currentY - 1][currentX - 1];
            if (currentY > 0 && currentX < grid[currentY].Length - 1) yield return grid[currentY - 1][currentX + 1];
            if (currentY < grid.Length - 1 && currentX > 0) yield return grid[currentY + 1][currentX - 1];
            if (currentY < grid.Length - 1 && currentX < grid[currentY].Length - 1) yield return grid[currentY + 1][currentX + 1];
        }
    }

    // A* implementation

    public sealed class Field
    {
        public Point Position { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Field? Parent { get; set; }

        public override string ToString() => $"{Position} = {Distance}";
    }

    public static Field? CrawlPath(Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth,
        Func<Point, Point, int>? distanceFunc = null, Func<int, int>? calcCost = null, bool includeDiagonals = false)
    {
        // default distance function is manhattan distance
        distanceFunc ??= (from, to) => from.ManhattanDistance(to);
        // default cost function is increment of 1
        calcCost ??= (cost) => cost + 1;

        var startField = new Field { Position = start, Cost = 0, Distance = distanceFunc(start, end) };
        var visitedFields = new List<Field>();
        var possibleFields = new List<Field>();

        possibleFields.Add(startField);

        while (possibleFields.Count > 0)
        {
            var current = possibleFields.MinBy(tile => tile.CostDistance)!;
            if (current.Position == end) return current;

            possibleFields.Remove(current);
            visitedFields.Add(current);

            foreach (var neighbor in current.Position.GetNeighborsFiltered(filter, maxHeight, maxWidth, includeDiagonals))
            {
                var neighborField = new Field { Position = neighbor, Cost = calcCost(current.Cost), Distance = distanceFunc(neighbor, end), Parent = current };

                if (visitedFields.Any(field => field.Position == neighborField.Position)) continue;
                if (possibleFields.Any(field => field.Position == neighborField.Position && field.CostDistance <= neighborField.CostDistance)) continue;

                possibleFields.Add(neighborField);
            }
        }
        return null;
    }
    public static List<Point>? FindPath(Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth,
        Func<Point, Point, int>? distanceFunc = null, Func<int, int>? calcCost = null, bool includeDiagonals = false)
    {
        return CrawlPath(start, end, filter, maxHeight, maxWidth, distanceFunc, calcCost, includeDiagonals) is Field path
            ? ReconstructPath(path)
            : null;
    }

    public static int? FindPathDistance(Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth,
        Func<Point, Point, int>? distanceFunc = null, Func<int, int>? calcCost = null, bool includeDiagonals = false)
    {
        return CrawlPath(start, end, filter, maxHeight, maxWidth, distanceFunc, calcCost, includeDiagonals)?.Cost;
    }

    public static List<Point> ReconstructPath(Field current)
    {
        var path = new List<Point>();
        while (current.Parent != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
}