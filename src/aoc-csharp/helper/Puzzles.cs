using System.Reflection;
namespace aoc_csharp;
public static class Puzzles
{
    /** Instance the type found by reflection */
    public static IPuzzle? GetPuzzle(Type targetType)
    {
        return (IPuzzle?)Activator.CreateInstance(targetType);
    }

    /** Gets all implementations in the csharp.impl folder */
    public static IEnumerable<Type> GetImplementedTypesFromNamespace()
    {
        return Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => string.Equals(t.Namespace, Config.ImplementationNamespace, StringComparison.Ordinal))
            .Where(t => !t.Name.Contains('<') && t.Name.StartsWith("Day"))
            .OrderBy(t => t.Name);
    }
}