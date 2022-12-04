using System.Text;
namespace aoc_csharp;
public static class Grids
{
    /** Maps a generic dictionary to a multidimensional grid with a given mapper function */
    public static TGrid[,] PointDictToGrid<TGrid, TMap>(Dictionary<Point, TMap> map, Func<TMap?, TGrid> mapper)
    {
        var minX = map.Keys.Min(p => p.X);
        var maxX = map.Keys.Max(p => p.X);
        var minY = map.Keys.Min(p => p.Y);
        var maxY = map.Keys.Max(p => p.Y);
        var width = maxX - minX + 1;
        var height = maxY - minY + 1;
        var grid = new TGrid[height, width];

        for (var lineIdx = 0; lineIdx < height; lineIdx++)
        {
            for (var posIdx = 0; posIdx < width; posIdx++)
            {
                grid[lineIdx, posIdx] = mapper(map.GetValueOrDefault(new Point(minX + posIdx, minY + lineIdx)));
            }
        }

        return grid;
    }

    /** Converts a generic multidimensional array to a printable string */
    public static string GridAsPrintable<TGrid>(TGrid[,] grid, Func<TGrid, string>? mapper = null, string? separator = null, int? padLength = null,
        string defaultWithoutMapper = "", string? lineSeparator = "\n")
    {
        return GridAsPrintable(grid.ToJaggedArray(), mapper, separator, padLength, defaultWithoutMapper, lineSeparator);
    }

    /** Converts a generic jagged array to a printable string */
    public static string GridAsPrintable<TGrid>(TGrid[][] grid, Func<TGrid, string>? mapper = null, string? separator = null, int? padLength = null,
        string defaultWithoutMapper = "", string? lineSeparator = "\n")
    {
        var result = new StringBuilder();
        var line = new StringBuilder();

        mapper ??= t => t?.ToString() ?? defaultWithoutMapper;

        for (var lineIdx = 0; lineIdx < grid.Length; lineIdx++)
        {
            line.Clear();
            for (var posIdx = 0; posIdx < grid[lineIdx].Length; posIdx++)
            {
                var val = grid[lineIdx][posIdx];
                var mapping = mapper?.Invoke(val) ?? defaultWithoutMapper;
                line.Append(padLength is not null
                    ? mapping.PadLeft(padLength.Value)
                    : mapping);

                if (separator is not null) line.Append(separator);
            }

            result.Append(line);
            if (lineSeparator is not null) result.Append(lineSeparator);
        }

        return result.ToString();
    }

    public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
    {
        var rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
        var rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
        var numberOfRows = rowsLastIndex - rowsFirstIndex + 1;

        var columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
        var columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
        var numberOfColumns = columnsLastIndex - columnsFirstIndex + 1;

        var jaggedArray = new T[numberOfRows][];
        for (var lineIdx = 0; lineIdx < numberOfRows; lineIdx++)
        {
            jaggedArray[lineIdx] = new T[numberOfColumns];

            for (var posIdx = 0; posIdx < numberOfColumns; posIdx++)
            {
                jaggedArray[lineIdx][posIdx] = twoDimensionalArray[lineIdx + rowsFirstIndex, posIdx + columnsFirstIndex];
            }
        }

        return jaggedArray;
    }

    /** Returns an enumerable of points between from and to going preferring to go horizontal then diagonal then vertical */
    public static IEnumerable<Point> Walk(Point from, Point to)
    {
        var points = new List<Point>();
        switch (from)
        {
            case var (x, y) when Math.Abs(x - to.X) == Math.Abs(y - to.Y):
                Range(from.X, to.X)
                    .Zip(Range(from.Y, to.Y))
                    .Select(pos => new Point(pos.First, pos.Second))
                    .ToList()
                    .ForEach(points.Add);
                break;
            case var (x, _) when x == to.X:
                Range(from.Y, to.Y)
                    .Select(y => new Point(x, y))
                    .ToList()
                    .ForEach(points.Add);
                break;
            case var (_, y) when y == to.Y:
                Range(from.X, to.X)
                    .Select(x => new Point(x, y))
                    .ToList()
                    .ForEach(points.Add);
                break;
            default:
                Printer.DebugMsg($"Invalid coordinates going from {from} to {to}");
                break;
        }

        return points;
    }
    
    /** Returns an enumerable of ints between from and to counting down if needed */
    public static IEnumerable<int> Range(int from, int to)
    {
        return from > to
            ? Enumerable.Range(to, from - to + 1)
            : Enumerable.Range(from, to - from + 1).Reverse();
    }
}