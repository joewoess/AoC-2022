namespace aoc_fsharp.helper.types

open System
open System.IO
open aoc_fsharp.helper.debug

[<AbstractClass>]
type public Puzzle() =
    member private this.input filename =
        lazy (File.ReadAllLines(filename) |> List.ofArray)

    member this.inputLines isDebug inputPath =
        (try
            Some(
                (match (this.input inputPath).IsValueCreated with
                 | true -> (this.input inputPath).Value
                 | false -> (this.input inputPath).Force())
            )
         with
         | :? FileNotFoundException as exc ->
             DebugMsg isDebug $"File not found: {exc.Message}"
             None
         | :? UnauthorizedAccessException as exc ->
             DebugMsg isDebug $"Could not open: {exc.Message}"
             None)


    abstract member SolveFirst : Boolean * String -> Option<string>
    abstract member SolveSecond : Boolean * String -> Option<string>
