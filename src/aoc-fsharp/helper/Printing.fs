module public aoc_fsharp.helper.printing

open System
open aoc_fsharp.helper.literals
open aoc_fsharp.helper.util
open aoc_fsharp.helper.records

let consoleWidth = 
    try 
        Console.WindowWidth - 1
    with
        | _ -> 105

let separator =
    [ (SeparatorChar, consoleWidth) ]
    |> Seq.collect repeat
    |> Array.ofSeq
    |> String

let inline PrintSeparator _ = printfn $"%s{separator}"

let PrintGreeting _ =
    PrintSeparator()
    printfn $"%s{GreetingMessage}"
    PrintSeparator()

let inline PrintResultHeader _ =
    printfn "|  Day  |         1st |         2nd |"

let PrintPuzzleSolution isDemo isDebug (available: Available) : unit =
    let first, second =
        match available with
        | { Implementation = Some impl
            InputFile = Some path } when isDemo |> not -> (defaultArg (impl.SolveFirst(isDebug, path)) UnknownMsg, defaultArg (impl.SolveSecond(isDebug, path)) UnknownMsg)
        | { Implementation = Some impl
            TestInputFile = Some path } when isDemo -> (defaultArg (impl.SolveFirst(isDebug, path)) UnknownMsg, defaultArg (impl.SolveSecond(isDebug, path)) UnknownMsg)
        | { Implementation = None
            InputFile = Some _ } when isDemo |> not -> (NoImplMsg, NoImplMsg)
        | { Implementation = None
            TestInputFile = Some _ } when isDemo -> (NoImplMsg, NoImplMsg)
        | { Implementation = Some _ } -> (NoInputMsg, NoInputMsg)
        | _ -> (NoImplOrInputMsg, NoImplOrInputMsg)

    printfn $"| Day%02i{available.DayNumber} |  %s{first.PadLeft(SolutionPadding)} |  %s{second.PadLeft(SolutionPadding)} |"
