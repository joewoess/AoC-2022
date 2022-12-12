using System.Numerics;

namespace aoc_csharp.puzzles;

public sealed class Day12 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var map = _input
            .Select(line => line.Select(c => new HeightValue(c)).ToArray())
            .ToArray();
        var grid =  new Dictionary<Point, HeightValue>();
        map.Select((line, lineIdx) => line.Select((h, posIdx) => (Pos: new Point(posIdx, lineIdx), Height: h)).ToArray())
            .SelectMany(x => x)
            .ToList()
            .ForEach(x => grid.Add(x.Pos, x.Height));
        
        var distanceMap = new Dictionary<Point, int>();
        var mapHeight = map.Length;
        var mapWidth = map[0].Length;
        
        Printer.DebugMsg($"The height map looks as follows:\n{Grids.GridAsPrintable(map)}");

        var start = grid.Where(x => x.Value.IsStart).Select(x => x.Key).Single();
        var goal = grid.Where(x => x.Value.IsEnd).Select(x => x.Key).Single();

        var currentPos = start;
        var lastPos = start;

        var pathToCheck = new Queue<Point>();
        distanceMap[currentPos] = 0;
        pathToCheck.Enqueue(currentPos);

        while(currentPos != goal && pathToCheck.Count > 0) {
            lastPos = currentPos;
            currentPos = pathToCheck.Dequeue();
            var neighbors = Day12.neighbors(currentPos, lastPos, grid, mapHeight, mapWidth);
            var possibleNextPos = new List<Point>();
            foreach (var pos in neighbors)
            {
                if(!distanceMap.ContainsKey(pos)) {
                    distanceMap[pos] = distanceMap[currentPos] + 1;
                    possibleNextPos.Add(pos);
                }
                else if(distanceMap[pos] < distanceMap[currentPos] + 1) {
                    distanceMap[pos] = distanceMap[currentPos] + 1;
                    possibleNextPos.Add(pos);
                }
            }
            possibleNextPos.ForEach(pos => pathToCheck.Enqueue(pos));
        }

        if(distanceMap.ContainsKey(goal))
            Printer.DebugMsg($"The distance map looks as follows:\n{Grids.GridAsPrintable(Grids.PointDictToGrid(distanceMap, (x) => x.ToString()))}");
        else
            Printer.DebugMsg($"No path found to the goal.");

        var minSteps = distanceMap[goal];
        Printer.DebugMsg($"Reached the goal in {minSteps} steps.");
        return minSteps.ToString();
    }

    public override string? SecondPuzzle()
    {
        return null;
    }

    private record HeightValue(int Value) {
        public HeightValue(char c) : this(Height(c)) { }
        public bool IsEnd => Value == (int)'E';
        public bool IsStart => Value == (int)'S';
        public char AsChar => Value switch {
            (int)'E' => 'E',
            (int)'S' => 'S',
            var v => (char)((int)'a' + v)
        };
        private static int Height(char c) => c switch {
            'E' => (int)'E',
            'S' => (int)'S',
            var v => (int)(v) - (int)'a'
        };
        public override string ToString() => AsChar.ToString();
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

    private static List<Point> neighbors(Point currentPos, Point lastPos, Dictionary<Point, HeightValue> grid, int maxHeight, int maxWidth) {
        var nextPos = new List<Point>();
        var (x, y) = currentPos;
        if(y + 1 < maxHeight) {
            var next = new Point(x, y + 1);
            if(grid[next].Value <= grid[currentPos].Value + 1 && next != lastPos)
                nextPos.Add(next);
        }
        if(y - 1 >= 0) {
            var next = new Point(x, y - 1);
            if(grid[next].Value <= grid[currentPos].Value + 1 && next != lastPos)
                nextPos.Add(next);
        }
        if(x + 1 < maxWidth) {
            var next = new Point(x + 1, y);
            if(grid[next].Value <= grid[currentPos].Value + 1 && next != lastPos)
                nextPos.Add(next);
        }
        if(x - 1 >= 0) {
            var next = new Point(x - 1, y);
            if(grid[next].Value <= grid[currentPos].Value + 1 && next != lastPos)
                nextPos.Add(next);
        }
        return nextPos;
    }
}