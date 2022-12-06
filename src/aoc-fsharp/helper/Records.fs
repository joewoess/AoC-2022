module public aoc_fsharp.helper.records

open aoc_fsharp.helper.types

type public Available =
    { DayNumber: int
      Implementation: Option<Puzzle>
      InputFile: Option<string>
      TestInputFile: Option<string> }
