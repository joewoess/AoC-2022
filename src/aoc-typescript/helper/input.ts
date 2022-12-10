import { readFileSync } from "fs";
import * as constants from "./constants";
import { asTwoDigitNumber } from "./util";

export function readInputOfDay(day: number, useDemoData: boolean = false, splitLines: boolean = true): string | string[] {
    const inputPath = useDemoData ? constants.InputPathDemo : constants.InputPathReal;
    const inputFilePath = `${inputPath}day${asTwoDigitNumber(day)}.txt`;
    // Read the input from the file
    const data = readFileSync(inputFilePath, "utf8");
    // If the data should be split into lines, split it
    if (splitLines) {
        return data.split("\n");
    }
    return data
}