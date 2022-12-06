module public aoc_fsharp.helper.reflection

open System.Reflection
open System.IO
open aoc_fsharp.helper.types
open aoc_fsharp.helper.util
open aoc_fsharp.helper.literals

let definedImplementations =
    Assembly.GetExecutingAssembly().DefinedTypes
    |> Array.ofSeq
    |> Array.filter (fun t -> t.IsSubclassOf(typeof<Puzzle>))
    |> Array.sortBy (fun t -> t.Name)

let GetImplForDay (dayNumber: int) : Option<Puzzle> =
    definedImplementations
    |> Array.tryFind (fun t -> t.Name = ClassNameFromDayNumber dayNumber)
    |> Option.map (fun t -> t.GetConstructor(Array.empty))
    |> Option.map (fun c -> c.Invoke(Array.empty))
    |> Option.map (fun p -> p :?> Puzzle)

let GetInputFileForDay isTest (dayNumber: int) : Option<string> =
    let path =
        (match isTest with
         | true -> InputPathFromDayNumber InputPathDemo dayNumber
         | false -> InputPathFromDayNumber InputPathReal dayNumber)

    match File.Exists path with
    | true -> Some(path)
    | false -> None
