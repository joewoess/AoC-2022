export { sum, asTwoDigitNumber };
function sum(items: number[]): number {
    return items.reduce((a, b) => a + b, 0);
}
function asTwoDigitNumber(value: number): string {
    return value.toString().padStart(2, "0");
}