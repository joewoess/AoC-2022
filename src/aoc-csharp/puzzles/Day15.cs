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
            .Select(line => (Sensor: line.Sensor, Beacon: line.Beacon, Distance: line.Sensor.ManhattanDistance(line.Beacon)))
            .ToList();

        var maxDistance = 0;
        var minX = 0;
        var maxX = 0;

        foreach (var (sensor, _, distance) in beaconData)
        {
            maxDistance = Math.Max(maxDistance, distance);
            minX = Math.Min(minX, sensor.X);
            maxX = Math.Max(maxX, sensor.X);
        }

        var coveredLine = 0;
        var lineToCheck = Config.IsDemo ? 10 : 2_000_000;

        for (int x = minX - maxDistance; x < maxX + maxDistance; x++)
        {
            var currentPoint = new Point(x, lineToCheck);
            if (beaconData.Any(pair => pair.Sensor == currentPoint || pair.Beacon == currentPoint)) continue;
            if (beaconData.Any(pair => pair.Sensor.ManhattanDistance(currentPoint) <= pair.Distance)) coveredLine++;
        }

        Printer.DebugMsg($"Pulsed the area for beacons");
        Printer.DebugMsg($"There are {coveredLine} positions not possible in line y={lineToCheck}");
        return coveredLine.ToString();
    }

    public override string? SecondPuzzle()
    {
        var beaconData = Data
            .Select(line => line.Split(' '))
            .Select(line => (SensorPart: (line[2], line[3]), BeaconPart: (line[^2], line[^1])))
            .Select(line => (SensorCoord: line.SensorPart.MapPairWith(CleanNumber), BeaconCoord: line.BeaconPart.MapPairWith(CleanNumber)))
            .Select(line => (Sensor: new Point(line.SensorCoord.First, line.SensorCoord.Second), Beacon: new Point(line.BeaconCoord.First, line.BeaconCoord.Second)))
            .Select(line => (Sensor: line.Sensor, Beacon: line.Beacon, Distance: line.Sensor.ManhattanDistance(line.Beacon)))
            .ToList();

        var maxCoord = Config.IsDemo ? 20 : 4_000_000;
        var tuningFrequencyMultiplier = 4_000_000;

        for (int y = 0; y < maxCoord; y++) {
            for (int x = 0; x < maxCoord; x++)
            {
                var currentPoint = new Point(x, y);
                if (beaconData.Any(pair => pair.Sensor == currentPoint || pair.Beacon == currentPoint)) continue;
                if (beaconData.Any(pair => pair.Sensor.ManhattanDistance(currentPoint) <= pair.Distance).Not())
                {
                    var tuningFrequency = tuningFrequencyMultiplier * x + y;
                    Printer.DebugMsg($"The only valid Position is at {currentPoint} with tuning frequency {tuningFrequency}");
                    return tuningFrequency.ToString();
                }
            }
        }

        return null;
    }

    private static int CleanNumber(string number)
    {
        return int.Parse(number.Substring(2).TrimEnd(',', ':'));
    }
}