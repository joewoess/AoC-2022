using aoc_csharp;
public readonly record struct Point(int X, int Y)
{
    public override string ToString() => $"({X},{Y})";
}

public interface IPuzzle
{
    string? FirstPuzzle();
    string? SecondPuzzle();
}