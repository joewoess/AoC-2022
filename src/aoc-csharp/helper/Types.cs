namespace aoc_csharp.helper;

/** Base interface for all puzzle implementations */
public interface IPuzzle
{
    string TypeName => GetType().Name;
    string DataFileName => $"{TypeName.ToLower()}.txt";

    bool WasInputSuccess();

    string FirstResult => WasInputSuccess() ? FirstPuzzle() ?? Config.NoResultMessage : Config.NoDataMessage;
    string SecondResult => WasInputSuccess() ? SecondPuzzle() ?? Config.NoResultMessage : Config.NoDataMessage;

    string? FirstPuzzle();
    string? SecondPuzzle();
}

/** Base for puzzles that parse the input line wise */
public abstract class PuzzleBaseLines : IPuzzle
{
    private readonly SuccessResult<string[]> _input;
    protected string[] Data => _input.Result ?? Array.Empty<string>();

    protected PuzzleBaseLines()
    {
        _input = this.GetInputLines();
    }

    public bool WasInputSuccess() => _input.Success;
    public abstract string? FirstPuzzle();
    public abstract string? SecondPuzzle();
}

/** Base for puzzles that parse the input as a whole */
public abstract class PuzzleBaseText : IPuzzle
{
    private readonly SuccessResult<string> _input;
    protected string Data => _input.Result ?? string.Empty;

    protected PuzzleBaseText()
    {
        _input = this.GetInput();
    }

    public bool WasInputSuccess() => _input.Success;
    public abstract string? FirstPuzzle();
    public abstract string? SecondPuzzle();
}

/** Dummy empty implementation */
public sealed class NoImplPuzzle : IPuzzle
{
    public bool WasInputSuccess() => true;
    public string TypeName => Config.NoSolutionMessage;
    public string? FirstPuzzle() => Config.NoSolutionMessage;
    public string? SecondPuzzle() => Config.NoSolutionMessage;
}

/** Helper struct for returning a success flag and a result */
public record struct SuccessResult<T>(bool Success, T? Result);