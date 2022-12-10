// import file system module
import * as fs from "fs";
import * as constants from "./helper/constants";
import * as input from "./helper/input";
import {sum} from "./helper/util";

const day = 1;
const useDemoData = true;

// Read the input from the file
const data = input.readInputOfDay(day, useDemoData, true) as string[];

// Print solution
console.log(`Puzzle for day ${day}:`);
console.log(`First: ${getMaxSum(data)}`);
console.log(`Second: ${getMaxSum(data, 3)}`);

// Implementation for day 01
function getMaxSum(input: string[], maxCount: number = 1): number {
    // Initialize the sums of each group to an empty array
    let sums: number[] = [];

    // Initialize the current group to an empty array
    let currentGroup: number[] = [];

    // Loop through the lines
    for (const line of input) {
        // Check if the line is empty
        if (line.trim() === "") {
            // If the line is empty, calculate the sum of the current group
            // and add it to the array of sums
            sums.push(sum(currentGroup));

            // Clear the current group
            currentGroup = [];
        } else {
            // If the line is not empty, add the number to the current group
            currentGroup.push(parseInt(line, 10));
        }
    }

    // Calculate the sum of the final group
    sums.push(sum(currentGroup));
    // sort in descending order
    sums.sort((a, b) => b - a);

    // Find the maximum sum in the array of sums
    const maxSum = sum(sums.slice(0, maxCount));

    // Return the maximum sum
    return maxSum;
};