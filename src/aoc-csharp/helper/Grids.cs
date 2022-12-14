using System.Text;

namespace aoc_csharp.helper;

public static class Grids
{
    /** Maps a generic dictionary to a multidimensional grid with a given mapper function */
    public static TGrid[,] AsGrid<TGrid, TMap>(this Dictionary<Point, TMap> map, Func<TMap?, TGrid> mapper)
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

    /** Converts a generic point dictionary to a printable string via PointDictToGrid */
    public static string AsPrintable<TGrid, TMap>(this Dictionary<Point, TMap> map, Func<TMap?, TGrid> mapper) => AsPrintable(AsGrid(map, mapper));

    /** Converts a generic multidimensional array to a printable string */
    public static string AsPrintable<TGrid>(this TGrid[,] grid, Func<TGrid, string>? mapper = null, string? separator = null, int? padLength = null,
        string defaultWithoutMapper = "", string? lineSeparator = "\n")
    {
        return AsPrintable(grid.AsJaggedArray(), mapper, separator, padLength, defaultWithoutMapper, lineSeparator);
    }

    /** Converts a generic jagged array to a printable string */
    public static string AsPrintable<TGrid>(this TGrid[][] grid, Func<TGrid, string>? mapper = null, string? separator = null, int? padLength = null,
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

    /** Converts a multidimensional array to a jagged array */
    public static T[][] AsJaggedArray<T>(this T[,] twoDimensionalArray)
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

    /** Non-generic version of PointDictToGrid for getting a grid with empty spaces as emptySpace */
    public static char[,] AsCharGrid(this Dictionary<Point, char> map, char emptySpace = '.') => AsGrid(map, c => c == 0 ? emptySpace : c);
}