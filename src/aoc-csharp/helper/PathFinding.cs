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

    private static List<TGrid> MapNeighbors<TMap, TGrid>(Dictionary<Point, TMap> map, Point currentPos, Func<Point, Point, TGrid> mapper, Func<Point, Point, bool> filter, int maxHeight, int maxWidth, bool includeDiagonals = false)
    {
        return currentPos.GetNeighborPoints(includeDiagonals)
            .Where(pos => pos.X >= 0 && pos.X < maxWidth)
            .Where(pos => pos.Y >= 0 && pos.Y < maxHeight)
            .Where(pos => filter(currentPos, pos))
            .Select(pos => mapper(currentPos, pos))
            .ToList();
    }

    private static IEnumerable<TGrid> GetNeighbors<TGrid>(TGrid[][] grid, int currentY, int currentX, bool includeDiagonals = false)
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

    // A Star implementation

    public class Field
    {
        public Point Position { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Field? Parent { get; set; }

        public override string ToString() => $"{Position} = {Distance}";
    }

    public static List<Point>? FindPath<TMap>(Dictionary<Point, TMap> map, Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth, bool includeDiagonals = false)
    {
        var startField = new Field { Position = start, Cost = 0, Distance = start.ManhattanDistance(end) };
        var visitedFields = new List<Field>();
        var possibleFields = new List<Field>();
        
        possibleFields.Add(startField);

        while (possibleFields.Count > 0)
        {
            var current = possibleFields.MinBy(tile => tile.CostDistance)!;
            if (current.Position == end) return ReconstructPath(current);

            possibleFields.Remove(current);
            visitedFields.Add(current);

            foreach (var neighbor in MapNeighbors(map, current.Position, (currentPos, neighborPos) => neighborPos, filter, maxHeight, maxWidth, includeDiagonals))
            {
                var neighborField = new Field { Position = neighbor, Cost = current.Cost + 1, Distance = neighbor.ManhattanDistance(end), Parent = current };

                if (visitedFields.Any(field => field.Position == neighborField.Position)) continue;
                if (possibleFields.Any(field => field.Position == neighborField.Position && field.CostDistance <= neighborField.CostDistance)) continue;

                possibleFields.Add(neighborField);
            }
        }
        return null;
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

    // public static List<Point> FindPathGeneric<TMap>(Dictionary<Point, TMap> map, Point start, Point end, Func<Point, Point, bool> filter, int maxHeight, int maxWidth, bool includeDiagonals = false)
    // {
    //     var startTile = new Field
    //     {
    //         Position = start,
    //     };

    //     var goalTile = new Field
    //     {
    //         Position = end
    //     };
    //     startTile.SetDistance(end);

    //     var visitedTiles = new List<Field>();
    //     var activeTiles = new List<Field>();
    //     activeTiles.Add(startTile);

    //     Field? checkingTile = null;

    //     while (activeTiles.Any())
    //     {
    //         checkingTile = activeTiles.OrderBy(x => x.CostDistance).First();

    //         if (checkingTile.Position == goalTile.Position)
    //         {
    //             // Found the goal
    //             break;
    //         }

    //         visitedTiles.Add(checkingTile);
    //         activeTiles.Remove(checkingTile);

    //         var walkableTiles = MapNeighbors(map, checkingTile, mapHeight, mapWidth);

    //         foreach (var walkableTile in walkableTiles)
    //         {
    //             //We have already visited this tile so we don't need to do so again!
    //             if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
    //                 continue;

    //             //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
    //             var existingTile = activeTiles.FirstOrDefault(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
    //             if (existingTile != null)
    //             {
    //                 if (existingTile.CostDistance > checkingTile.CostDistance)
    //                 {
    //                     activeTiles.Remove(existingTile);
    //                     activeTiles.Add(walkableTile);
    //                 }
    //             }
    //             else
    //             {
    //                 //We've never seen this tile before so add it to the list. 
    //                 activeTiles.Add(walkableTile);
    //             }
    //         }
    //     }

    //     if (checkingTile?.X != goalTile.X || checkingTile?.Y != goalTile.Y)
    //     {
    //         Printer.DebugMsg($"Failed to find a path to the goal tile from ({start.X}, {start.Y})");
    //         continue;
    //     }
    //     var isNewMin = checkingTile?.CostDistance < (minGoalTile?.CostDistance ?? int.MaxValue);
    //     Printer.DebugMsg($"Found the goal tile from startpoint ({start.X}, {start.Y}) with distance {checkingTile?.CostDistance}. New min? {isNewMin}");

    //     if (isNewMin)
    //     {
    //         minGoalTile = checkingTile;
    //     }
    // }
}