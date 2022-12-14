namespace aoc_csharp.puzzles;

public sealed class Day08 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var grid = Data
            .Select(line => line.Select(c => (int)c).ToArray()).ToArray()
            .ToArray();

        var height = grid.Length;
        var width = grid[0].Length;

        Printer.DebugMsg($"Grid is {height}x{width} with values:\n{grid.AsPrintable()} ");

        var lastTop = new int[width];
        var lastBottom = new int[width];
        Array.Fill(lastTop, -1);
        Array.Fill(lastBottom, -1);
        var visibleFromOutside = new bool[height, width].AsJaggedArray();
        for (int y = 0; y < height; y++)
        {
            int lastLeft = -1;
            int lastRight = -1;
            for (int x = 0; x < width; x++)
            {
                // vertical
                if (grid[y][x] > lastTop[x])
                {
                    Printer.DebugMsg($"VisibleFromTop {grid[y][x]} At ({y},{x})");
                    lastTop[x] = grid[y][x];
                    visibleFromOutside[y][x] = true;
                }
                if (grid[^(y + 1)][x] > lastBottom[x])
                {
                    Printer.DebugMsg($"VisibleFromBottom {grid[^(y + 1)][x]} At ({^(y + 1)},{x})");
                    lastBottom[x] = grid[^(y + 1)][x];
                    visibleFromOutside[^(y + 1)][x] = true;
                }

                //horizontal
                if (grid[y][x] > lastLeft)
                {
                    Printer.DebugMsg($"VisibleFromLeft {grid[y][x]} At ({y},{x})");
                    lastLeft = grid[y][x];
                    visibleFromOutside[y][x] = true;
                }
                if (grid[y][^(x + 1)] > lastRight)
                {
                    Printer.DebugMsg($"VisibleFromRight {grid[y][^(x + 1)]} At ({y},{^(x + 1)})");
                    lastRight = grid[y][^(x + 1)];
                    visibleFromOutside[y][^(x + 1)] = true;
                }
            }
        }

        Printer.DebugMsg($"Visible trees are:\n{visibleFromOutside.AsPrintable((b) => (b ? "1" : "0"))} ");

        var visibleTrees = visibleFromOutside.SelectMany(row => row).Count(v => v);
        Printer.DebugMsg($"Found {visibleTrees} trees visible from outside.");

        return visibleTrees.ToString();
    }

    public override string? SecondPuzzle()
    {
        var grid = Data
            .Select(line => line.Select(c => (int)c).ToArray())
            .ToArray();
        
        var height = grid.Length;
        var width = grid[0].Length;
        
        Printer.DebugMsg($"Grid is {height}x{width} with values:\n{grid.AsPrintable()} ");

        var maxScore = 0;

        // skip the edges, because there will always be a 0 score there
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                var score = ScoreForTree(grid, y, x);
                if (score > maxScore)
                {
                    maxScore = score;
                }
            }
        }

        return maxScore.ToString();
    }

    private static int ScoreForTree(int[][] grid, int y, int x)
    {
        var leftScore = 0;
        var rightScore = 0;
        var topScore = 0;
        var bottomScore = 0;

        var offset = 1;
        for (offset = 1; y - offset >= 0; offset++)
        {
            topScore++;
            if (grid[y - offset][x] >= grid[y][x])
            {
                break;
            }
        }
        for (offset = 1; x - offset >= 0; offset++)
        {
            leftScore++;
            if (grid[y][x - offset] >= grid[y][x])
            {
                break;
            }
        }
        for (offset = 1; x + offset < grid[y].Length; offset++)
        {
            rightScore++;
            if (grid[y][x + offset] >= grid[y][x])
            {
                break;
            }
        }
        for (offset = 1; y + offset < grid.Length; offset++)
        {
            bottomScore++;
            if (grid[y + offset][x] >= grid[y][x])
            {
                break;
            }
        }
        Printer.DebugMsg($"({y},{x}) has scores: {topScore}, {leftScore}, {rightScore}, {bottomScore} for a total of {leftScore * rightScore * topScore * bottomScore}");
        return leftScore * rightScore * topScore * bottomScore;
    }
}