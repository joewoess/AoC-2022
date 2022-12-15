namespace aoc_csharp.puzzles;

public sealed class Day12 : PuzzleBaseLines
{
    public override bool SecondIsLongRunning() => true;
    private int MapHeight => Data.Length;
    private int MapWidth => Data[0].Length;

    public override string? FirstPuzzle()
    {
        var grid = ParseHeightValuesToDict();

        var start = grid.Where(entry => entry.Value.IsStart).Select(e => e.Key).Single();
        var goal = grid.Where(entry => entry.Value.IsEnd).Select(e => e.Key).Single();

        var foundPath = PathFinding.FindPath(start, goal,
            (curr, next) => grid[curr].CanWalkTo(grid[next]),
            MapHeight, MapWidth,
            PointerExtensions.ManhattanDistance,
            (cost) => cost + 1,
            false);

        if (foundPath == null)
        {
            Printer.DebugMsg($"Failed to find a path to the goal tile from ({start.X}, {start.Y})");
            return null;
        }

        // pretty print the path
        var travelledPath = foundPath.ToDictionary(pos => pos, pos => grid[pos]);
        Printer.DebugMsg($"The path looks like this:\n{travelledPath.AsPrintable((val) => (val == default ? '.' : val.Symbol))}");

        var directionPath = foundPath
            .PairWithNext()
            .Select(path => (path.From, symbol: path.From.GetDirectionCharTowards(path.To)))
            .ToDictionary(pair => pair.From, pair => pair.symbol);

        directionPath[foundPath.Last()] = 'E';
        Printer.DebugMsg($"Alternative display is:\n{directionPath.AsCharGrid().AsPrintable()}");

        Printer.DebugMsg($"Found shortest path with {foundPath.Count} steps.");
        return foundPath.Count.ToString();
    }

    public override string? SecondPuzzle()
    {
        var grid = ParseHeightValuesToDict(c => c == 'S' ? 'a' : c);

        var possibleStarts = grid
            .Where(entry => entry.Value.Height == 0)
            .Select(e => e.Key)
            .ToList();
        Printer.DebugPrintExcerpt(possibleStarts, "Possible starts are: ");
        var goal = grid.Where(entry => entry.Value.IsEnd).Select(e => e.Key).Single();

        int minDistance = int.MaxValue;

        foreach (var (start, idx) in possibleStarts.Select((s, idx) => (s, idx)))
        {
            // only use FindPathDistance so we avoid ReconstructPath execution
            var foundPath = PathFinding.FindPathDistance(start, goal,
                (curr, next) => grid[curr].CanWalkTo(grid[next]),
                MapHeight, MapWidth,
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

    /** Parse the input into a dictionary of positions and height values. Can use charMapper to remove the given starting point */
    private Dictionary<Point, HeightValue> ParseHeightValuesToDict(Func<char, char>? charMapper = null)
    {
        charMapper ??= (c) => c;
        var grid = new Dictionary<Point, HeightValue>();
        Data
            .Select(line => line.Select(c => new HeightValue(charMapper(c))).ToArray())
            .Select((line, lineIdx) => line.Select((h, posIdx) => (Pos: new Point(posIdx, lineIdx), Height: h)).ToArray())
            .SelectMany(x => x)
            .ToList()
            .ForEach(x => grid.Add(x.Pos, x.Height));
        return grid;
    }

    private readonly record struct HeightValue(int Height, char Symbol)
    {
        private const int HeightBase = 'a';
        public HeightValue(char c) : this(ParseHeightFromSymbol(c), c) { }
        public bool IsStart => Symbol == 'S';
        public bool IsEnd => Symbol == 'E';

        private static int ParseHeightFromSymbol(char ch) => ch switch
        {
            'S' => 0,
            'E' => 25,
            _ => ch - HeightBase
        };

        public bool CanWalkTo(HeightValue other) => other.Height <= Height + 1;

        public override string ToString() => Symbol.ToString();
    }
}