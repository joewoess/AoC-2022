using System.Reflection;
namespace aoc_csharp;
public static class Puzzles
{
    /** Gets all puzzles in the csharp.puzzles folder by naming convention 'DayXX.cs' */
    public static List<IPuzzle> ImplementedPuzzles { get; } =
        GetImplementedTypesFromNamespace()
            .Select(GetPuzzle)
            .Where(puzzle => puzzle != null)
            .Cast<IPuzzle>()
            .ToList();

    public static NoImplPuzzle NoImplementation { get; } = new NoImplPuzzle();

    /** Instance the type found by reflection */
    public static IPuzzle? GetPuzzle(Type targetType)
    {
        return (IPuzzle?)Activator.CreateInstance(targetType);
    }

    /** Gets all implementations in the csharp.puzzles folder that use the IPuzzles interface */
    public static IEnumerable<Type> GetImplementedTypesFromNamespace()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => string.Equals(t.Namespace, Config.ImplementationNamespace, StringComparison.OrdinalIgnoreCase))
            //.Where(t => t is IPuzzle)
            .Where(t => !t.Name.Contains('<') && t.Name.StartsWith("Day"))
            .OrderBy(t => t.Name);
    }

    public static Dictionary<int, List<IPuzzle>> PuzzleImplementationDict { get; } =
        Util.Range(1, Config.MaxChallengeDays)
            .ToDictionary(
                day => day,
                day => ImplementedPuzzles.Where(puzzle => int.Parse(string.Join("", puzzle.TypeName.Where(char.IsDigit))) == day).ToList() ?? new List<IPuzzle> { NoImplementation });
}