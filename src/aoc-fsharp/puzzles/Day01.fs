namespace aoc_fsharp.puzzles

open aoc_fsharp.helper.types

type Day01() =
    inherit Puzzle()

    // split sequence every time there is a blank line
    let splitOnBlankLines (lines: List<string>) : List<List<string>> =
        lines
        |> List.fold
            (fun (acc: List<List<string>>) (line: string) ->
                match line with
                | "" -> List.empty :: acc
                | _ ->
                    match acc with
                    | [] -> [[line]]
                    | _ -> (line :: acc.Head) :: acc.Tail)
            []
        |> List.map List.rev

    let sumCalories (collection: List<int>) : int =
        collection
        |> List.sum

    let maxCalories (collection: List<int>) : int =
        collection
        |> List.max
    let maxThreeCalories (collection: List<int>) : int =
        collection
        |> List.sortDescending
        |> List.take 3
        |> List.sum

    override this.SolveFirst(isDebug, inputPath) =
        this.inputLines isDebug inputPath
        |> Option.map
            (fun lines ->
                lines
                |> splitOnBlankLines
                |> List.map (fun elves -> 
                    elves
                    |> List.map (fun it -> it.Trim())
                    |> List.map (fun it -> it |> int)
                    |> sumCalories)
                |> maxCalories)
        |> Option.map (fun maxCalories -> $"%i{maxCalories}")
        |> Option.orElse None

    override this.SolveSecond(isDebug, inputPath) =
        this.inputLines isDebug inputPath
        |> Option.map
            (fun lines ->
                lines
                |> splitOnBlankLines
                |> List.map (fun elves -> 
                    elves
                    |> List.map (fun it -> it.Trim())
                    |> List.map (fun it -> it |> int)
                    |> sumCalories)
                |> maxThreeCalories)
        |> Option.map (fun maxCalories -> $"%i{maxCalories}")
        |> Option.orElse None
