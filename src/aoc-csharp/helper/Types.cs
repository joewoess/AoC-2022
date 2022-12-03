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

public abstract class PuzzleBaseLines : IPuzzle
{
    protected readonly string[] _input;

    public PuzzleBaseLines()
    {
        _input = Input.GetInputLines(this);
    }
    public abstract string? FirstPuzzle();
    public abstract string? SecondPuzzle();
}

public abstract class PuzzleBaseText : IPuzzle
{
    protected readonly string _input;

    public PuzzleBaseText()
    {
        _input = Input.GetInput(this);
    }
    public abstract string? FirstPuzzle();
    public abstract string? SecondPuzzle();
}