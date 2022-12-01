# AoC-2022

My solutions to the coding challenge [adventofcode](https://adventofcode.com/2022) written in whatever coding language my heart desired that day :)

All implemented solutions will be linked in the [Challenges table](##Challenges)  with their respective languages.

---

## Challenges

| Day | Challenge | C# | F# | Kotlin | Rust | Python |
| ---: |:---------| :-------:| :-------:| :-------:| :-------:| :-------:|
|  1  | [Calorie Counting](https://adventofcode.com/2022/day/1) | [Csharp](src/csharp/puzzles/Day01.cs) | FSharp | Kotlin | Rust | Python
|  2  | [Challenge 02](https://adventofcode.com/2022/day/2)  | Csharp | FSharp | Kotlin | Rust | Python
|  3  | [Challenge 03](https://adventofcode.com/2022/day/3)  | Csharp | FSharp | Kotlin | Rust | Python
|  4  | [Challenge 04](https://adventofcode.com/2022/day/4)  | Csharp | FSharp | Kotlin | Rust | Python
|  5  | [Challenge 05](https://adventofcode.com/2022/day/5)  | Csharp | FSharp | Kotlin | Rust | Python
|  6  | [Challenge 06](https://adventofcode.com/2022/day/6)  | Csharp | FSharp | Kotlin | Rust | Python
|  7  | [Challenge 07](https://adventofcode.com/2022/day/7)  | Csharp | FSharp | Kotlin | Rust | Python
|  8  | [Challenge 08](https://adventofcode.com/2022/day/8)  | Csharp | FSharp | Kotlin | Rust | Python
|  9  | [Challenge 09](https://adventofcode.com/2022/day/9)  | Csharp | FSharp | Kotlin | Rust | Python
| 10  | [Challenge 10](https://adventofcode.com/2022/day/10) | Csharp | FSharp | Kotlin | Rust | Python
| 11  | [Challenge 11](https://adventofcode.com/2022/day/11) | Csharp | FSharp | Kotlin | Rust | Python
| 12  | [Challenge 12](https://adventofcode.com/2022/day/12) | Csharp | FSharp | Kotlin | Rust | Python
| 13  | [Challenge 13](https://adventofcode.com/2022/day/13) | Csharp | FSharp | Kotlin | Rust | Python
| 14  | [Challenge 14](https://adventofcode.com/2022/day/14) | Csharp | FSharp | Kotlin | Rust | Python
| 15  | [Challenge 15](https://adventofcode.com/2022/day/15) | Csharp | FSharp | Kotlin | Rust | Python
| 16  | [Challenge 16](https://adventofcode.com/2022/day/16) | Csharp | FSharp | Kotlin | Rust | Python
| 17  | [Challenge 17](https://adventofcode.com/2022/day/17) | Csharp | FSharp | Kotlin | Rust | Python
| 18  | [Challenge 18](https://adventofcode.com/2022/day/18) | Csharp | FSharp | Kotlin | Rust | Python
| 19  | [Challenge 19](https://adventofcode.com/2022/day/19) | Csharp | FSharp | Kotlin | Rust | Python
| 20  | [Challenge 20](https://adventofcode.com/2022/day/20) | Csharp | FSharp | Kotlin | Rust | Python
| 21  | [Challenge 21](https://adventofcode.com/2022/day/21) | Csharp | FSharp | Kotlin | Rust | Python
| 22  | [Challenge 22](https://adventofcode.com/2022/day/22) | Csharp | FSharp | Kotlin | Rust | Python
| 23  | [Challenge 23](https://adventofcode.com/2022/day/23) | Csharp | FSharp | Kotlin | Rust | Python
| 24  | [Challenge 24](https://adventofcode.com/2022/day/24) | Csharp | FSharp | Kotlin | Rust | Python
| 25  | [Challenge 25](https://adventofcode.com/2022/day/25) | Csharp | FSharp | Kotlin | Rust | Python

---

## Usage

Depending on the language version, all that is need is to go into the respective folder and
use their common build tool to run it.

```
# these are the valid optional parameters for all implementations (can be freely combined)
 --demo      ... Use test input instead of puzzle input
 --debug     ... Show debug output
 --last      ... Show last challenge commited
 01 4 20     ... Number list specifying certain days to output 
```
```zsh
# using gradle for kotlin
# or for the included gradle wrapper use gradlew (or ./gradlew on windows)
gradle run --args='01'
gradlew run --args='01'

# using dotnet for c# and f#
dotnet run -- --demo

# using python3 for rust
cargo run -- 01

# using python3 for python
python3 main.py --debug
```

## Languages used in this challenge

* C# 11.0 (.NET 7)
* F# 7.0 (.NET 7)

## Folder structure 

```
+---data
|   +---demo
|       - day01.txt
|       - ...
|   +---real
|       - day01.txt
|       - ...
+---src
|   +---aoc-csharp
|   +---aoc-fsharp
+---dump
|   - AoC.sln
- README.md
- .gitignore
```


## Sample output

```log
------------------------------------------------------------------------------
AdventOfCode Runner for 2022
Challenge at: https://adventofcode.com/2022/
Author: Johannes Wöß
Written in C# 11 / .NET 7
------------------------------------------------------------------------------
|  Day  |         1st |         2nd |
| Day01 |        1624 |        1653 |
Could not find solution for day Day02
```