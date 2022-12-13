module main

open aoc_fsharp.helper.literals
open aoc_fsharp.helper.printing
open aoc_fsharp.helper.debug
open aoc_fsharp.helper.records
open aoc_fsharp.helper.reflection
open aoc_fsharp.helper.util

[<EntryPoint>]
let main (args: string []) : int =
    let IsDebug = args |> Array.contains "--debug"
    let IsDemo = args |> Array.contains "--demo"
    let OnlyLast = args |> Array.contains "--last"
    let OnlyFirst = args |> Array.contains "--first"
    let OnlySecond = args |> Array.contains "--second"

    let dayNumberParams =
        args
        |> Array.filter (fun it -> it.StartsWith("--") |> not)
        |> Array.map (fun it -> it |> int)
        |> Array.filter (fun it -> it |> betweenIncl (1, MaxDays))
        |> Array.sort

    let OnlyNumbers = dayNumberParams |> Array.isEmpty |> not

    let allDays =
        { 1 .. MaxDays }
        |> Seq.toArray
        |> Array.map
            (fun idx ->
                { DayNumber = idx
                  Implementation = (GetImplForDay idx)
                  InputFile = GetInputFileForDay false idx
                  TestInputFile = GetInputFileForDay true idx })
    
    //DebugMsg IsDebug $"Day01: %A{Array.head allDays}"

    let whatIsAvailable =
        allDays
        |> Array.filter
            (fun avail ->
                avail.Implementation.IsSome
                || avail.TestInputFile.IsSome
                || avail.InputFile.IsSome)

    let lastOption =
        (match whatIsAvailable |> Array.isEmpty with
         | true -> None
         | false -> Some(whatIsAvailable |> Array.last))


    let whatToDisplay =
        (match lastOption with
         | None -> Array.empty
         | Some last ->
             (match (OnlyLast, OnlyNumbers) with
              | true, true ->
                  whatIsAvailable
                  |> Array.filter (fun avail -> dayNumberParams |> Array.contains avail.DayNumber)
                  |> Array.append (Array.singleton last)
              | true, false -> Array.singleton last
              | false, true ->
                  whatIsAvailable
                  |> Array.filter (fun avail -> dayNumberParams |> Array.contains avail.DayNumber)
              | false, false -> whatIsAvailable))
        |> Array.sortBy (fun avail -> avail.DayNumber)
        |> Array.distinct

    PrintGreeting()

    DebugMsg IsDebug $"IsDemo[%b{IsDemo}] IsDebug[%b{IsDebug}] ShowLast[%b{OnlyLast}] OnlyFirst[%b{OnlyFirst}] OnlySecond[%b{OnlySecond}] DayNumbers[%A{dayNumberParams}]"
    DebugMsg IsDebug $"Available  #=%A{MapToDayNumber whatIsAvailable}"
    DebugMsg IsDebug $"ToDisplay  #=%A{MapToDayNumber whatToDisplay}"

    if OnlyLast then
        printfn "Show last entry"

    if OnlyNumbers then
        printfn $"Show entries for days: %A{dayNumberParams}"

    PrintResultHeader()

    for available in whatToDisplay do
        PrintPuzzleSolution IsDemo IsDebug available

    PrintSeparator()
    0
