namespace aoc_csharp.puzzles;

public sealed class Day15 : PuzzleBaseLines
{

    public override string? FirstPuzzle()
    {
        var beaconData = Data
            .Select(line => line.Split(' '))
            .Select(line => (SensorPart: (line[2], line[3]), BeaconPart: (line[^2], line[^1])))
            .Select(line => (SensorCoord: line.SensorPart.MapPairWith(CleanNumber), BeaconCoord: line.BeaconPart.MapPairWith(CleanNumber)))
            .Select(line => (Sensor: new Point(line.SensorCoord.First, line.SensorCoord.Second), Beacon: new Point(line.BeaconCoord.First, line.BeaconCoord.Second)))
            .ToList();

        var caveDict = InitCaveDictionary(beaconData);

        Printer.DebugMsg($"Beacon response from cave looks like this:{Environment.NewLine}{caveDict.AsCharGrid().AsPrintable()}");

        var maxDistance = 0;

        foreach (var (sensor, beacon) in beaconData)
        {
            maxDistance = beacon.ManhattanDistance(sensor);
            for (int offsetY = -maxDistance; offsetY <= maxDistance; offsetY++)
            {
                var offsetLength = maxDistance - Math.Abs(offsetY);
                for (int offsetX = -offsetLength; offsetX <= offsetLength; offsetX++)
                {
                    var currentPoint = new Point(sensor.X + offsetX, sensor.Y + offsetY);
                    if (caveDict.ContainsKey(currentPoint)) continue;
                    caveDict[currentPoint] = '#';
                }
            }
        }

        Printer.DebugMsg($"Pulsed the area for beacons");

        var lineToCheck = Config.IsDemo ? 10 : 2_000_000;
        var caveWithNotPossible = caveDict.AsCharGrid();
        Printer.DebugMsg($"Covered areas in cave:{Environment.NewLine}{caveWithNotPossible.AsPrintable()}");

        var line = string.Join("", caveDict.Where(entry => entry.Key.Y == lineToCheck).Select(entry => entry.Value).ToList());
        Printer.DebugMsg($"Line y={lineToCheck} looks like this:{Environment.NewLine}{line}");

        var positionsNotPossibleInLine = caveDict.Count(entry => entry.Key.Y == lineToCheck && entry.Value == '#');
        Printer.DebugMsg($"There are {positionsNotPossibleInLine} positions not possible in line y={lineToCheck}");
        return positionsNotPossibleInLine.ToString();
    }

    public override string? SecondPuzzle()
    {
        return null; ;
    }

    private static int CleanNumber(string number)
    {
        return int.Parse(number.Substring(2).TrimEnd(',', ':'));
    }
    private static Dictionary<Point, char> InitCaveDictionary(List<(Point, Point)> beaconData)
    {
        var caveDict = new Dictionary<Point, char>();

        foreach (var (sensor, beacon) in beaconData)
        {
            caveDict[sensor] = 'S';
            caveDict[beacon] = 'B';
        }

        return caveDict;
    }
}