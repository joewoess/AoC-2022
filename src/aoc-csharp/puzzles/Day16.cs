using System.Numerics;

namespace aoc_csharp.puzzles;

public sealed class Day16 : PuzzleBaseLines
{

    private const int MinuteMax = 30;

    public override string? FirstPuzzle()
    {
        var beaconData = Data
            .Select(line => line.Split(' '))
            .Select(line => new Valve(line[1], CleanNumber(line[4]), line[9..]))
            .ToDictionary(valve => valve.Name, valve => valve);


        var bestActions = FindBestActions(beaconData, "AA", 0, 0);
        var totalPressure = 0;

        foreach (var action in bestActions)
        {
            // do action
        }

        Printer.DebugMsg($"The total pressure after 30 mins is {totalPressure}");
        return totalPressure.ToString();
    }

    public override string? SecondPuzzle()
    {
        return null;
    }

    private static int CleanNumber(string number)
    {
        return int.Parse(number.Substring(5).TrimEnd(',', ':', ';'));
    }

    private static List<Actions> FindBestActions(Dictionary<string, Valve> valves, string currentValve, int totalPressure = 0, int minute = 0) {
        var bestActions = new List<Actions>();
        var bestPressure = 0;
        var bestMinute = 0;
        var current = valves[currentValve];
        if(!current.IsOpen) {
            bestActions = FindBestActions(valves, currentValve, totalPressure, minute + 1);
        }
        foreach (var valve in valves[currentValve].Targets) {
            if (valves[valve].IsOpen) {
                var newPressure = totalPressure + valves[valve].Pressure;
                var newMinute = minute + 1;
                if (newMinute > MinuteMax) {
                    continue;
                }
                var newActions = FindBestActions(valves, valve, newPressure, newMinute);
                if (newPressure > bestPressure) {
                    bestPressure = newPressure;
                    bestMinute = newMinute;
                    bestActions = newActions;
                }
            } else {
                var newPressure = totalPressure - valves[valve].Pressure;
                var newMinute = minute + 1;
                if (newMinute > MinuteMax) {
                    continue;
                }
                var newActions = FindBestActions(valves, valve, newPressure, newMinute);
                if (newPressure > bestPressure) {
                    bestPressure = newPressure;
                    bestMinute = newMinute;
                    bestActions = newActions;
                }
            }
        }
        return bestActions;
    }

    private sealed record class Valve(string Name, int Pressure, string[] Targets, bool IsOpen = false, int OpenedAt = 0);
    private enum OperateValve { Open, Close, JumpTo };
    private sealed record class Actions(string Valve, OperateValve Operation, string? JumpTo = null);
}