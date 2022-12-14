namespace aoc_csharp.puzzles;

public sealed class Day13 : PuzzleBaseLines
{
    public override string? FirstPuzzle()
    {
        var listPairs = Data
            .Where(Util.HasContent)
            .Select(ParseListFromLine)
            .Chunk(2)
            .Select(chunk => (Left: chunk[0], Right: chunk[1]))
            .ToList();

        Printer.DebugMsg($"Found {listPairs.Count} pairs of lists");

        var correctIndices = new List<int>();

        listPairs
            .Select((pair, idx) => (pair, idx))
            .Where((pairWithIdx) => IsCorrectOrder(pairWithIdx.pair.Left, pairWithIdx.pair.Right) == CorrectOrder.Correct)
            .ToList()
            .ForEach(pairWithIdx => correctIndices.Add(pairWithIdx.idx + 1));

        Printer.DebugMsg($"Found {correctIndices.Count} correct indices: ({string.Join(", ", correctIndices)})");
        Printer.DebugMsg($"Sum of them is {correctIndices.Sum()}");
        return correctIndices.Sum().ToString();
    }

    public override string? SecondPuzzle()
    {
        var listPairs = Data
            .Where(Util.HasContent)
            .Select(ParseListFromLine)
            .Select(list => list)
            .ToList();

        Printer.DebugMsg($"Found {listPairs.Count} lists.");

        var firstControlDivider = ListOrValue.NestValueInDepth(2, 2);
        var secondControlDivider = ListOrValue.NestValueInDepth(6, 2);
        listPairs.AddRange(new[] { firstControlDivider, secondControlDivider });

        Printer.DebugMsg($"Added two divider lists to the end: {listPairs[^2]} and {listPairs[^1]}");

        listPairs = listPairs.Order().ToList();
        Printer.DebugMsg("Ordered lists:");
        listPairs.ForEach(list => Printer.DebugMsg($"{list}"));

        var idxFirst = listPairs.IndexOf(firstControlDivider) + 1;
        var idxSecond = listPairs.IndexOf(secondControlDivider) + 1;
        var decipherKey = idxFirst * idxSecond;

        Printer.DebugMsg($"Found decipher keys at {idxFirst} and {idxSecond} => {decipherKey}");
        return decipherKey.ToString();
    }

    private static CorrectOrder IsCorrectOrder(ListOrValue left, ListOrValue right)
    {
        if (left.IsInteger && right.IsInteger)
        {
            if (left.Val < right.Val) return CorrectOrder.Correct;
            if (left.Val > right.Val) return CorrectOrder.InCorrect;
            if (left.Val == right.Val) return CorrectOrder.Continue;
        }
        else if (left.IsList && right.IsList)
        {
            var isCorrectOrder = CorrectOrder.Continue;
            var listIdx = 0;
            while (isCorrectOrder == CorrectOrder.Continue)
            {
                if (listIdx >= left.NestedList?.Length && listIdx < right.NestedList?.Length)
                    return CorrectOrder.Correct;
                if (listIdx < left.NestedList?.Length && listIdx >= right.NestedList?.Length)
                    return CorrectOrder.InCorrect;
                if (listIdx >= left.NestedList?.Length && listIdx >= right.NestedList?.Length)
                    return CorrectOrder.Continue;
                isCorrectOrder = IsCorrectOrder(left.NestedList![listIdx], right.NestedList![listIdx]);

                if (isCorrectOrder != CorrectOrder.Continue)
                {
                    return isCorrectOrder;
                }

                listIdx++;
            }
        }
        else
        {
            if (left.IsInteger && right.IsList)
            {
                return IsCorrectOrder(new ListOrValue(null, new[] { left }), right);
            }

            if (left.IsList && right.IsInteger)
            {
                return IsCorrectOrder(left, new ListOrValue(null, new[] { right }));
            }
        }

        // should never happen
        Printer.DebugMsg($"IsCorrectOrder could not determine if it was correct. Defaulting to InCorrect");
        return CorrectOrder.InCorrect;
    }

    private static ListOrValue ParseListFromLine(String line)
    {
        var idx = 0;
        return ParseLineRecursive(line, ref idx, 0).NestedList![0];
    }

    private static ListOrValue ParseLineRecursive(String line, ref int index, int depth)
    {
        var result = new List<ListOrValue>();
        var startIndex = index;

        while (index < line.Length)
        {
            switch (line[index])
            {
                case '[':
                    if (index > startIndex)
                    {
                        // Push values before the list
                        var values = line.Substring(startIndex, index - startIndex)
                            .Split(',')
                            .Where(Util.HasContent)
                            .Select(int.Parse)
                            .Select(i => new ListOrValue(i, null));
                        values.ToList().ForEach(result.Add);
                    }

                    index++;
                    // Push nested list
                    var list = ParseLineRecursive(line, ref index, depth + 1);
                    result.Add(list);
                    startIndex = index;
                    continue;
                case ']':
                    if (index > startIndex)
                    {
                        // Push values before the list
                        var values = line.Substring(startIndex, index - startIndex)
                            .Split(',')
                            .Where(Util.HasContent)
                            .Select(int.Parse)
                            .Select(i => new ListOrValue(i, null));
                        values.ToList().ForEach(result.Add);
                    }

                    index++;
                    return new ListOrValue(null, result.ToArray());
                default:
                    index++;
                    break;
            }
        }

        if (depth == 0)
            return new ListOrValue(null, result.ToArray());
        throw new Exception("There was no enclosing brackets. String is invalid.");
    }

    private record ListOrValue(int? Val, ListOrValue[]? NestedList) : IComparable<ListOrValue>
    {
        public bool IsInteger => Val != null;
        public bool IsList => NestedList != null;

        public override string ToString() => IsInteger
            ? ((int)Val!).ToString()
            : NestedList.ToListString();

        public static ListOrValue NestValueInDepth(int val, int depth = 0)
        {
            if (depth <= 0) return new ListOrValue(val, null);
            return new ListOrValue(null, new[] { NestValueInDepth(val, depth - 1) });
        }

        public int CompareTo(ListOrValue? other)
        {
            return other == null
                ? 0
                : IsCorrectOrder(this, other!) switch
                {
                    CorrectOrder.Correct => -1,
                    CorrectOrder.InCorrect => 1,
                    CorrectOrder.Continue => 0,
                    _ => 0
                };
        }
    }

    private enum CorrectOrder
    {
        Correct,
        InCorrect,
        Continue
    };
}