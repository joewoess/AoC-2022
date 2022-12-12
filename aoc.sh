#!/bin/bash

# This script is used to run the AoC-2022 project.

# Set default values for the flags
csharp=false
fsharp=false
python=false
typescript=false
rust=false

last=false
debug=false
demo=false

# Parse the flags
while [[ "$1" =~ ^- && ! "$1" == "--" ]]; do case $1 in
  -h | --help )
    echo "--------------------------------------------------------------------------------"
    echo "AdventOfCode Runner for 2022" 
    echo "Challenge at: https://adventofcode.com/2022/"
    echo "Author: Johannes Wöß"
    echo "Written in F# 7 / .NET 7"
    echo "--------------------------------------------------------------------------------"
    echo "Usage: ./aoc.sh [options] [arguments]"
    echo "Arguments: "
    echo "  --last  |     Run the last solution that was run"
    echo "  --debug |     Run the solution in debug mode"
    echo "  --demo  |     Run the solution in demo mode"
    echo "  [nums]  |     numbers of days to run the solution for (e.g. 1 2 3)"
    echo "Options:"
    echo "  -h      |     --help"
    echo "  -cs     |     --csharp"
    echo "  -fs     |     --fsharp"
    echo "  -py     |     --python"
    echo "  -ts     |     --typescript"
    echo "  -r      |     --rust"
    exit
    ;;
  -cs | --csharp )
    csharp=true
    ;;
  -fs | --fsharp )
    fsharp=true
    ;;
  -py | --python )
    python=true
    ;;
  -ts | --typescript )
    typescript=true
    ;;
  -r | --rust )
    rust=true
    ;;
  --last )
    last=true
    ;;
  --debug )
    debug=true
    ;;
  --demo )
    demo=true
    ;;
esac; shift; done
if [[ "$1" == '--' ]]; then shift; fi

exec_command=""

# Execute the appropriate command based on the input flags
if $csharp; then
  # Execute csharp solution with the given arguments
  cd "./src/aoc-csharp/" || exit 1
  exec_command="dotnet run $@"
elif $fsharp; then
  # Execute fsharp solution with the given arguments
  cd "./src/aoc-fsharp/" || exit 1
  exec_command="dotnet run $@"
elif $typescript; then
  # Execute typescript solution with the given arguments
  cd "./src/aoc-typescript/" || exit 1
  exec_command="ts-node $@"
elif $python; then
  # Execute python solution with the given arguments
  cd "./src/aoc-python/" || exit 1
  exec_command="python3 $@" 
elif $rust; then
  # Execute rust solution with the given arguments
  cd "./src/aoc-rust/" || exit 1
  exec_command="cargo run $@"
else
  # Default to csharp solution if no flags are given
  cd "./src/aoc-csharp/" || exit 1
  echo "Running csharp solution as a fallback because no flags were given."
  exec_command="dotnet run $@"
fi

# Add the universal flags like --last to the command
if $last; then
  # Execute csharp solution with the given arguments
  exec_command="$exec_command --last"
fi
if $debug; then
  # Execute fsharp solution with the given arguments
  exec_command="$exec_command --debug"
fi
if $demo; then
  # Execute typescript solution with the given arguments
  exec_command="$exec_command --demo"
fi

# Execute the command
echo "Executing command: $exec_command in $PWD"
$exec_command