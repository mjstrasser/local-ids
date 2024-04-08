const LocalId = require('./LocalId');


describe('newId()', () => {
    test('length includes check digit', () => {
        expect(LocalId.newId().length).toEqual(16);
    });
    test('crude performance test', () => {
        let start = performance.now();
        for (let i = 0; i < 100000; i++) {
            LocalId.newId();
        }
        let end = performance.now();
        expect(end - start).toBeLessThan(400);
    });
});

describe('isValid()', () => {
    test('returns false if the string is the wrong length', () => {
        expect(LocalId.isValid("0123456789ABCDEFG")).toBe(false);
        expect(LocalId.isValid("0123456789ABCDE")).toBe(false);
    });
    test('returns false if the string contains invalid characters', () => {
        expect(LocalId.isValid("0123456789ABCDE?")).toBe(false);
    });
    test('returns true if the check character is correct', () => {
        expect(LocalId.isValid("0000000000000000")).toBe(true);
        expect(LocalId.isValid("A00000000000000A")).toBe(true);
        expect(LocalId.isValid("000b00000000000b")).toBe(true);
        expect(LocalId.isValid("000000000X00000X")).toBe(true);
        expect(LocalId.isValid("0123456789ABCDEh")).toBe(true);
    });
    test('returns false if the check character is incorrect', () => {
        expect(LocalId.isValid("0123456789ABCDEj")).toBe(false);
    });
    test('returns true for many values from a file', () => {
        const fs = require('fs'),
            path = require('path'),
            readline = require('readline');

        const filePath = path.join(__dirname, '../valid-ids.txt');
        const fileStream = fs.createReadStream(filePath);
        const rl = readline.createInterface({
            input: fileStream,
            crlfDelay: Infinity,
        });
        rl.on('line', (stringId) => {
            expect(LocalId.isValid(stringId)).toBe(true);
        });
    });
});
