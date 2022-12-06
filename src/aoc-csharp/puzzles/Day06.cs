namespace aoc_csharp.puzzles;

public sealed class Day06 : PuzzleBaseText
{
    public override string? FirstPuzzle()
    {
        var uniqueSequenceLength = 4;
        int pos = uniqueSequenceLength;
        for (; pos < _input.Length; pos++)
        {
            var seq = _input[(pos - uniqueSequenceLength)..pos];
            Printer.DebugMsg($"Sequence: {seq}");
            if(seq.Distinct().Count() == seq.Length)
            {
                Printer.DebugMsg($"Found unique sequence {seq} at position {pos}.");
                break;
            }
        }
        return pos.ToString();
    }

    public override string? SecondPuzzle()
    {
        var uniqueSequenceLength = 14;
        int pos = uniqueSequenceLength;
        for (; pos < _input.Length; pos++)
        {
            var seq = _input[(pos - uniqueSequenceLength)..pos];
            Printer.DebugMsg($"Sequence: {seq}");
            if(seq.Distinct().Count() == seq.Length)
            {
                Printer.DebugMsg($"Found unique sequence {seq} at position {pos}.");
                break;
            }
        }
        return pos.ToString();
    }
}