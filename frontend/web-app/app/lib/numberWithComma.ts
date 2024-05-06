/**
 * This function takes a number and returns a string representation of the number 
 * with commas separating the thousands. 
 * For example, the number 1000000 would be converted to the string "1,000,000".
 *
 * @param {number} amount - The number to be formatted with commas.
 * @returns {string} The formatted string.
 */

export function numberWithCommas(amount: number) {
    // Convert the number to a string and use a regular expression to insert commas.
    // The regular expression /\B(?=(\d{3})+(?!\d))/g matches a position where a comma should be inserted.
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}