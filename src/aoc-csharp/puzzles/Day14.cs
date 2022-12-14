namespace aoc_csharp.puzzles;

public sealed class Day14 : PuzzleBaseLines
{
    private static readonly Point SandTrickleStart = new(500, 0);
    private const char SandStart = '+';
    private const char Sand = 'o';
    private const char Rock = '#';
    private const char Overflow = '~';

    public override string? FirstPuzzle()
    {
        var rockData = ExtractPoints(Data);
        Printer.DebugMsg($"Found {rockData.Count} lines of rock data");

        var caveDict = InitCaveDictionary(rockData);
        var abyssLevel = caveDict.Keys.Max(p => p.Y);

        Printer.DebugMsg($"Endless abyss below {abyssLevel}");
        Printer.DebugMsg($"Cave looks like this without sand:{Environment.NewLine}{caveDict.AsCharGrid().AsPrintable()}");


        var unitsOfSand = TrickleSand(caveDict, abyssLevel, false);

        // also print overflow
        var overFlowUnit = SandTrickleStart;
        while (overFlowUnit.Y < abyssLevel + 2)
        {
            overFlowUnit = FallOneStep(caveDict, overFlowUnit);
            caveDict[overFlowUnit] = Overflow;
        }

        Printer.DebugMsg($"Cave looks like this with all the sand:{Environment.NewLine}{caveDict.AsCharGrid().AsPrintable()}");
        Printer.DebugMsg($"There was {unitsOfSand} units of sand that landed in the cave");
        return unitsOfSand.ToString();
    }

    public override string? SecondPuzzle()
    {
        var rockData = ExtractPoints(Data);
        Printer.DebugMsg($"Found {rockData.Count} lines of rock data");

        var caveDict = InitCaveDictionary(rockData);
        var floorLevel = caveDict.Keys.Max(p => p.Y) + 2;

        Printer.DebugMsg($"There is bedrock at {floorLevel}");
        Printer.DebugMsg($"Cave looks like this without sand:{Environment.NewLine}{caveDict.AsCharGrid().AsPrintable()}");

        var unitsOfSand = TrickleSand(caveDict, floorLevel, true);

        Printer.DebugMsg($"Cave looks like this with all the sand:{Environment.NewLine}{caveDict.AsCharGrid().AsPrintable()}");
        Printer.DebugMsg($"There was {unitsOfSand} units of sand that landed in the cave");
        return unitsOfSand.ToString();
    }

    // Input parsing
    private static List<List<Point>> ExtractPoints(string[] data)
    {
        return data
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split(" -> ")
                .Select(coord => coord.SplitAndMapToPair(int.Parse))
                .Select(intPair => new Point(intPair.First, intPair.Second))
                .ToList())
            .ToList();
    }

    private static Dictionary<Point, char> InitCaveDictionary(List<List<Point>> rockData)
    {
        var caveDict = new Dictionary<Point, char>
        {
            [SandTrickleStart] = SandStart
        };

        foreach (var rockLine in rockData)
        {
            foreach (var (from, to) in rockLine.PairWithNext())
            {
                from.WalkDirectlyTowards(to, allowDiagonal: false, includeStart: true)
                    .ToList()
                    .ForEach(step => caveDict[step] = Rock);
            }
        }

        return caveDict;
    }

    // Sand simulation
    private static int TrickleSand(Dictionary<Point, char> caveDict, int floorLevel, bool hasBedrock)
    {
        var unitsOfSand = 0;
        var newSandUnit = SandTrickleStart;

        while (newSandUnit.Y < floorLevel)
        {
            var nextStep = FallOneStep(caveDict, newSandUnit, hasBedrock ? floorLevel : null);
            if (newSandUnit == nextStep)
            {
                caveDict[newSandUnit] = Sand;
                newSandUnit = SandTrickleStart;
                unitsOfSand++;

                if (nextStep == SandTrickleStart)
                {
                    break;
                }
            }
            else
            {
                newSandUnit = nextStep;
            }
        }

        return unitsOfSand;
    }

    private static Point FallOneStep(Dictionary<Point, char> map, Point point, int? bedrock = null)
    {
        var down = point.StepInDirection(Direction.Down);
        var downLeft = point.StepInDirection(Direction.DownLeft);
        var downRight = point.StepInDirection(Direction.DownRight);

        // add bedrock if we're at the bottom
        if (bedrock.HasValue && bedrock == down.Y)
        {
            map[down] = Rock;
            map[downLeft] = Rock;
            map[downRight] = Rock;
        }

        if (!map.ContainsKey(down)) return down;
        if (!map.ContainsKey(downLeft)) return downLeft;
        if (!map.ContainsKey(downRight)) return downRight;

        return point;
    }
}