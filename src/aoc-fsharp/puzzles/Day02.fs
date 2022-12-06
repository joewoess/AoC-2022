namespace aoc_fsharp.puzzles

open aoc_fsharp.helper.types

type rps = Rock | Paper | Scissors
type game = Loss | Draw | Win

type Day02() =
    inherit Puzzle()

    let valueOfHand (hand: rps) : int
        = match hand with
          | Rock -> 1
          | Paper -> 2
          | Scissors -> 3

    let valueOfGame (result: game) : int
        = match result with
          | Loss -> 0
          | Draw -> 3
          | Win -> 6
    
    let charToHand (ch : char) : rps
        = match ch with
            | 'A' -> Rock
            | 'B' -> Paper
            | 'C' -> Scissors
            | 'X' -> Rock
            | 'Y' -> Paper
            | 'Z' -> Scissors
            | _ -> failwith "Invalid hand"

    let charToGame (ch : char) : game
        = match ch with
            | 'X' -> Loss
            | 'Y' -> Draw
            | 'Z' -> Win
            | _ -> failwith "Invalid game"

    let winningHandAgainst (hand: rps)
        = match hand with
            | Rock -> Paper
            | Paper -> Scissors
            | Scissors -> Rock

    let resolveGame (hand1 : rps) (hand2 : rps) : game
        = match hand1 with
            | h when hand1 = hand2 -> Draw
            | h when h = winningHandAgainst hand2 -> Win
            | _ -> Loss

    let desiredPlay (hand1 : rps) (result : game) : rps
        = match result with
            | Draw -> hand1
            | Win -> winningHandAgainst hand1
            | Loss -> hand1 |> winningHandAgainst |> winningHandAgainst

    let splitIntoPair (line: string) : (char * char) =
        let parts = line.Split(' ')
        (parts.[0] |> char, parts.[1] |> char)

    override this.SolveFirst(isDebug, inputPath) =
        this.inputLines isDebug inputPath
        |> Option.map
            (fun lines ->
                lines
                |> List.map splitIntoPair
                |> List.map (fun (hand1, hand2) -> (charToHand hand1, charToHand hand2))
                |> List.map (fun (hand1, hand2) -> (valueOfHand hand2) + (valueOfGame (resolveGame hand1 hand2)))
                |> List.sum)
        |> Option.map (fun score -> $"%i{score}")
        |> Option.orElse None

    override this.SolveSecond(isDebug, inputPath) =
        this.inputLines isDebug inputPath
        |> Option.map
            (fun lines ->
                lines
                |> List.map splitIntoPair
                |> List.map (fun (hand1, result) -> (charToHand hand1, charToGame result))
                |> List.map (fun (hand1, result) -> (valueOfGame result) + valueOfHand (desiredPlay hand1 result))
                |> List.sum)
        |> Option.map (fun score -> $"%i{score}")
        |> Option.orElse None
