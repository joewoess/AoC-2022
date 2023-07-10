namespace aoc_fsharp.puzzles

open aoc_fsharp.helper.types
open System.Collections.Generic

type public Monkey = 
    { MonkeyNumber: int
      Items: Queue<int>
      Operation: int -> int
      TestDivisor: int
      TargetIfTrue: int
      TargetIfFalse: int }



type Day11() = 
    inherit Puzzle()

    // split sequence every time there is a blank line
    // let splitOnBlankLines (lines: List<string>) : List<List<string>> =
    //     lines
    //     |> List.fold
    //         (fun (acc: List<List<string>>) (line: string) ->
    //             match line with
    //             | "" -> List.empty :: acc
    //             | _ ->
    //                 match acc with
    //                 | [] -> [[line]]
    //                 | _ -> (line :: acc.Head) :: acc.Tail)
    //         []
    //     |> List.map List.rev

    // let maxTwoMonkeyInspections (dataChunk: List<int>) : int =
    //     dataChunk
    //     |> List.sortDescending
    //     |> List.take 2
    //     |> List.sum

    // let asDictionary (collection: List<Monkey>) : Map<Monkey, int> =
    //     collection
    //     |> List.fold
    //         (fun (acc: Map<T, int>) (element: T) ->
    //             acc.Add(element, 0))
    //         Map.empty

    // let parseLine (line: string) : Some =
    //     match line with 
    //     | "this is a line" -> Some 1
    //     | "" -> None

    override this.SolveFirst(isDebug, inputPath) =
        this.inputLines isDebug inputPath
        |> Option.map (fun lines -> lines |> List.map (fun line -> line.Trim()))
        // this.inputLines isDebug inputPath
        // |> Option.map (fun lines -> lines |> splitOnBlankLines)
            // (fun lines ->
            //     lines
            //     |> splitOnBlankLines
            //     |> List.map (fun monkeyData -> 
            //         monkeyData
            //         |> List.map (fun it -> it.Trim())
            //         |> List.map (fun it -> it |> int))
            //     |> maxTwoMonkeyInspections)
        // |> Option.map (fun (first, second) -> $"%i{first * second}")
        // |> Option.orElse None
        None

    override this.SolveSecond(isDebug, inputPath) =
        None
