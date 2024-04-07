const LocalId = require('./local-id');


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
        expect(LocalId.isValid("P5O5bH8Hbnm9f4i")).toBe(false);
    });
    test('returns false if the string contains invalid characters', () => {
        expect(LocalId.isValid("@COCqGLPDiCfWeOc")).toBe(false);
    });
    test('returns true if the check character is correct', () => {
        expect(LocalId.isValid("zKy8DqCjTyaiKiSi")).toBe(false);
    });
    test('returns false if the check character is incorrect', () => {
        expect(LocalId.isValid("zKy8DqCjTyaiKiSj")).toBe(false);
    });
});