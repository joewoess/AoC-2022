module public aoc_fsharp.helper.util

open System.IO
open aoc_fsharp.helper.records

let inline ClassNameFromDayNumber (dayNumber: int) = $"Day%02i{dayNumber}"

let inline InputPathFromDayNumber (inputPath: string) (dayNumber: int) =
    Path.Combine(inputPath, $"day%02i{dayNumber}.txt")

let inline FirstCharUpper (s: string) =
    match s.Length with
    | 0 -> s
    | _ -> s.Substring(0, 1).ToUpper() + s.Substring(1)

let inline betweenIncl (min, max) num = (num >= min) && (num <= max)

let rec repeat (item, n) =
    seq {
        match n > 0 with
        | true ->
            yield item
            yield! repeat (item, n - 1)
        | false -> "" |> ignore
    }

let inline MapToDayNumber (days: Available []) =
    days |> Array.map (fun it -> it.DayNumber)
