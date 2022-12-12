using System.Numerics;

namespace aoc_csharp.puzzles;

public sealed class Day12 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        // var map = _input
        //     .Select(line => line.Select(c => new HeightValue(c)).ToArray())
        //     .ToArray();
        // var grid = new Dictionary<Point, HeightValue>();
        // map.Select((line, lineIdx) => line.Select((h, posIdx) => (Pos: new Point(posIdx, lineIdx), Height: h)).ToArray())
        //     .SelectMany(x => x)
        //     .ToList()
        //     .ForEach(x => grid.Add(x.Pos, x.Height));

        // var mapHeight = map.Length;
        // var mapWidth = map[0].Length;

        // Printer.DebugMsg($"The height map looks as follows:\n{Grids.GridAsPrintable(map)}");

        // var start = grid.Where(x => x.Value.IsStart).Select(x => x.Key).Single();
        // var goal = grid.Where(x => x.Value.IsEnd).Select(x => x.Key).Single();

        // var startTile = new Tile
        // {
        //     X = start.X,
        //     Y = start.Y
        // };

        // var goalTile = new Tile()
        // {
        //     X = goal.X,
        //     Y = goal.Y
        // };

        // startTile.SetDistance(goal.X, goal.Y);

        // var activeTiles = new List<Tile>();
        // activeTiles.Add(startTile);
        // var visitedTiles = new List<Tile>();
        // Tile? checkTile = null;

        // while (activeTiles.Any())
        // {
        //     checkTile = activeTiles.OrderByDescending(x => x.CostDistance).Last();

        //     if (checkTile.X == goalTile.X && checkTile.Y == goalTile.Y)
        //     {
        //         Printer.DebugMsg($"Found the goal tile at {checkTile.X}, {checkTile.Y}!");
        //         break;
        //     }

        //     visitedTiles.Add(checkTile);
        //     activeTiles.Remove(checkTile);

        //     var walkableTiles = GetWalkableTiles(grid, checkTile, goalTile, mapHeight, mapWidth);

        //     foreach (var walkableTile in walkableTiles)
        //     {
        //         //We have already visited this tile so we don't need to do so again!
        //         if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
        //             continue;

        //         //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
        //         if (activeTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
        //         {
        //             var existingTile = activeTiles.First(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
        //             if (existingTile.CostDistance > checkTile.CostDistance)
        //             {
        //                 activeTiles.Remove(existingTile);
        //                 activeTiles.Add(walkableTile);
        //             }
        //         }
        //         else
        //         {
        //             //We've never seen this tile before so add it to the list. 
        //             activeTiles.Add(walkableTile);
        //         }
        //     }
        // }

        // if(checkTile?.X == goalTile.X && checkTile?.Y == goalTile.Y)
        // {
        //     var tile = checkTile;
        //     while(true)
        //     {
        //         var pos = new Point(tile.X, tile.Y);
        //         Printer.DebugMsg($"Visited {grid[pos]} at {pos} with cost {tile.Cost} and distance {tile.Distance}");

        //         tile = tile.Parent;
        //         if(tile == null)
        //         {
        //             break;
        //         }
        //     }
        // }

        // var minSteps = checkTile?.CostDistance ?? 0;
        // Printer.DebugMsg($"Reached the goal in {minSteps} steps.");
        // return minSteps.ToString();
        return null;
    }

    public override string? SecondPuzzle()
    {
        var grid = new Dictionary<Point, HeightValue>();
        _input
            // ignore given start because we need to find all possible starts
            .Select(line => line.Select(c => new HeightValue(c == 'S' ? 'a' : c)).ToArray())
            .Select((line, lineIdx) => line.Select((h, posIdx) => (Pos: new Point(posIdx, lineIdx), Height: h)).ToArray())
            .SelectMany(x => x)
            .ToList()
            .ForEach(x => grid.Add(x.Pos, x.Height));

        var mapHeight = _input.Length;
        var mapWidth = _input[0].Length;

        var possibleStarts = grid.Where(entry => entry.Value.Height == 0).Select(e => e.Key).ToList();
        Printer.DebugMsg($"Possible starts are: {string.Join(", ", possibleStarts)}");
        var goal = grid.Where(entry => entry.Value.IsEnd).Select(e => e.Key).Single();

        Tile? minGoalTile = null;

        foreach (var start in possibleStarts)
        {
            var startTile = new Tile
            {
                X = start.X,
                Y = start.Y
            };

            var goalTile = new Tile
            {
                X = goal.X,
                Y = goal.Y
            };

            startTile.SetDistance(goal.X, goal.Y);

            var activeTiles = new List<Tile>();
            var visitedTiles = new List<Tile>();
            activeTiles.Add(startTile);

            Tile? checkTile = null;

            while (activeTiles.Any())
            {
                checkTile = activeTiles.OrderByDescending(x => x.CostDistance).Last();

                if (checkTile.X == goalTile.X && checkTile.Y == goalTile.Y)
                {
                    // Found the goal
                    break;
                }

                visitedTiles.Add(checkTile);
                activeTiles.Remove(checkTile);

                var walkableTiles = GetWalkableTiles(grid, checkTile, goalTile, mapHeight, mapWidth);

                foreach (var walkableTile in walkableTiles)
                {
                    //We have already visited this tile so we don't need to do so again!
                    if (visitedTiles.Any(x => x.X == walkableTile.X && x.Y == walkableTile.Y))
                        continue;

                    //It's already in the active list, but that's OK, maybe this new tile has a better value (e.g. We might zigzag earlier but this is now straighter). 
                    var existingTile = activeTiles.FirstOrDefault(x => x.X == walkableTile.X && x.Y == walkableTile.Y);
                    if (existingTile != null)
                    {
                        if (existingTile.CostDistance > checkTile.CostDistance)
                        {
                            activeTiles.Remove(existingTile);
                            activeTiles.Add(walkableTile);
                        }
                    }
                    else
                    {
                        //We've never seen this tile before so add it to the list. 
                        activeTiles.Add(walkableTile);
                    }
                }
            }

            if (checkTile?.X != goalTile.X || checkTile?.Y != goalTile.Y)
            {
                Printer.DebugMsg($"Failed to find a path to the goal tile from ({start.X}, {start.Y})");
                continue;
            }
            var isNewMin = checkTile?.CostDistance < (minGoalTile?.CostDistance ?? int.MaxValue);                
            Printer.DebugMsg($"Found the goal tile from startpoint ({start.X}, {start.Y}) with distance {checkTile?.CostDistance}. New min? {isNewMin}");

            if (isNewMin)
            {
                minGoalTile = checkTile;
            }
        }

        var minSteps = minGoalTile?.CostDistance ?? 0;
        Printer.DebugMsg($"Found shortest path with {minSteps} steps.");
        return minSteps.ToString();
    }

    private record struct HeightValue(int Height, char Symbol)
    {
        private const int HeightBase = (int)'a';
        public HeightValue(char c) : this(ParseSymbol(c), c) {}
        public HeightValue(int h) : this(h, ParseHeight(h)) {}
        public bool IsStart => Symbol == 'S';
        public bool IsEnd => Symbol == 'E';
        public static char ParseHeight(int height) => (char)(height + HeightBase);
        public static int ParseSymbol(char ch) => ch switch
        {
            'S' => 0,
            'E' => 25,
            var c => (int)(c) - HeightBase
        };

        public bool CanWalkTo(HeightValue other) => other.Height <= Height + 1;

        public override string ToString() => Symbol.ToString();
    }

    private class Tile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Cost { get; set; }
        public int Distance { get; set; }
        public int CostDistance => Cost + Distance;
        public Tile? Parent { get; set; }

        public void SetDistance(int targetX, int targetY)
        {
            this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
        }

        public override string ToString() => $"({X}, {Y}) = {Distance}";
    }

    private static List<Tile> GetWalkableTiles(Dictionary<Point, HeightValue> grid, Tile currentTile, Tile targetTile, int maxHeight, int maxWidth)
    {
        var possibleTiles = new List<Tile>()
        {
            new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
            new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1},
            new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
            new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
        };

        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

        return possibleTiles
                .Where(tile => tile.X >= 0 && tile.X < maxWidth)
                .Where(tile => tile.Y >= 0 && tile.Y < maxHeight)
                .Where(tile => grid[new Point(currentTile.X, currentTile.Y)].CanWalkTo(grid[new Point(tile.X, tile.Y)]))
                .ToList();
    }
}