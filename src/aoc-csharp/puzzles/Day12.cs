using System.Numerics;
namespace aoc_csharp.puzzles;

public sealed class Day12 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var grid = new Dictionary<Point, HeightValue>();
        _input
            .Select(line => line.Select(c => new HeightValue(c)).ToArray())
            .Select((line, lineIdx) => line.Select((h, posIdx) => (Pos: new Point(posIdx, lineIdx), Height: h)).ToArray())
            .SelectMany(x => x)
            .ToList()
            .ForEach(x => grid.Add(x.Pos, x.Height));

        var mapHeight = _input.Length;
        var mapWidth = _input[0].Length;

        var start = grid.Where(entry => entry.Value.IsStart).Select(e => e.Key).Single();
        var goal = grid.Where(entry => entry.Value.IsEnd).Select(e => e.Key).Single();

        var foundPath = PathFinding.FindPath(start, goal,
                                            (curr, next) => grid[curr].CanWalkTo(grid[next]),
                                            mapHeight, mapWidth,
                                            PointerExtensions.ManhattanDistance,
                                            (cost) => cost + 1,
                                            false);
        if (foundPath == null)
        {
            Printer.DebugMsg($"Failed to find a path to the goal tile from ({start.X}, {start.Y})");
            return null;
        }
        else
        {
            // pretty pring the path
            var travelledPath = foundPath.ToDictionary(pos => pos, pos => grid[pos]);
            Printer.DebugMsg($"The path looks like this:\n{Grids.GridAsPrintable(Grids.PointDictToGrid(travelledPath, (val) => (val == default ? '.' : val.Symbol)))}");

            var directionPath = foundPath
                                    .Zip(foundPath.Skip(1), (curr, next) => (curr, symbol: curr.GetDirectionCharTowards(next)))
                                    .ToDictionary(x => x.curr, x => x.symbol);
            directionPath[foundPath.Last()] = 'E';
            Printer.DebugMsg($"Alternative display is:\n{Grids.GridAsPrintable(Grids.PointDictToGrid(directionPath, (val) => (val == default ? '.' : val)))}");


            Printer.DebugMsg($"Found shortest path with {foundPath.Count} steps.");
            return foundPath.Count.ToString();
        }
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
        Printer.DebugPrintExcerpt(possibleStarts, "Possible starts are: ");
        var goal = grid.Where(entry => entry.Value.IsEnd).Select(e => e.Key).Single();

        int minDistance = int.MaxValue;

        foreach (var (start, idx) in possibleStarts.Select((s, idx) => (s, idx)))
        {
            // only use FinbdPathDistance so we avoid ReconstructPath execution
            var foundPath = PathFinding.FindPathDistance(start, goal,
                                                (curr, next) => grid[curr].CanWalkTo(grid[next]),
                                                mapHeight, mapWidth,
                                                PointerExtensions.ManhattanDistance,
                                                (cost) => cost + 1,
                                                false);
            if (foundPath == null)
            {
                //Printer.DebugMsg($"Failed to find a path to the goal tile from ({start.X}, {start.Y})");
                continue;
            }
            if (foundPath < minDistance)
            {
                Printer.DebugMsg($"Found new min path starting at ({start.X}, {start.Y}) with distance {foundPath}. {possibleStarts.Count - idx} more to go.");
                minDistance = (int)foundPath;
            }
        }

        Printer.DebugMsg($"Found shortest path with {minDistance} steps.");
        return minDistance.ToString();
    }

    private record struct HeightValue(int Height, char Symbol)
    {
        private const int HeightBase = (int)'a';
        public HeightValue(char c) : this(ParseSymbol(c), c) { }
        public HeightValue(int h) : this(h, ParseHeight(h)) { }
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
}