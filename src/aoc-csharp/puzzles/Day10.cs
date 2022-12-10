namespace aoc_csharp.puzzles;

public sealed class Day10 : PuzzleBaseLines
{
    private static readonly Func<int, bool> IsControlCycle = (int cycle) => cycle == 20 || (cycle > 20 && (cycle - 20) % 40 == 0);
    public override string? FirstPuzzle()
    {
        var instructions = _input
            .Select(line => line.Split(" ").ToArray())
            .Select(line => new Instruction(line[0], line.Length > 1 ? int.Parse(line[1]) : 0))
            .ToArray();

        Printer.DebugMsg($"There are {instructions.Length} instructions.");

        var regX = 1;
        var cycle = 1;
        var signalSum = 0;

        foreach (var instruction in instructions)
        {
            for (var offset = 0; offset < instruction.Cycles; offset++)
            {
                var signalStrength = cycle * regX;
                regX += instruction.Cmd switch {
                    "addx" when offset == instruction.Cycles - 1 => instruction.Amount,
                    _ => 0
                };
                if (IsControlCycle(cycle))
                {
                    Printer.DebugMsg($"Cycle {cycle}: X={regX}, Cmd: {instruction.Cmd} {instruction.Amount}, Signal strength: {signalStrength}");
                    signalSum += signalStrength;
                }
                cycle++;
            }

        }

        Printer.DebugMsg($"Total signal strength sum is {signalSum}.");

        return signalSum.ToString();
    }

    public override string? SecondPuzzle()
    {
        var instructions = _input
            .Select(line => line.Split(" ").ToArray())
            .Select(line => new Instruction(line[0], line.Length > 1 ? int.Parse(line[1]) : 0))
            .ToArray();

        var regX = 1;
        var cycle = 1;
        var signalSum = 0;
        var display = new Dictionary<Point, char>();

        foreach (var instruction in instructions)
        {
            for (var offset = 0; offset < instruction.Cycles; offset++)
            {
                var displayCycle = cycle - 1;
                var point = new Point(displayCycle % 40, displayCycle / 40);
                display[point] = Math.Abs(displayCycle % 40 - regX) < 2 ? '#' : '.';

                var signalStrength = cycle * regX;
                regX += instruction.Cmd switch {
                    "addx" when offset == instruction.Cycles - 1 => instruction.Amount,
                    _ => 0
                };
                if (IsControlCycle(cycle))
                {
                    signalSum += signalStrength;
                }
                cycle++;
            }

        }

        var crtDisplay = Grids.GridAsPrintable(Grids.PointDictToGrid(display, val => val));
        Printer.DebugMsg($"CRT display shows:\n{crtDisplay}.");

        return signalSum.ToString();
    }

    private record Instruction(string Cmd, int Amount)
    {
        public int Cycles => Cmd == "addx" ? 2 : 1;
    }
}