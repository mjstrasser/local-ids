# Local IDs

Experiments in creating useful, locally-unique random IDs, in multiple languages.

A local ID is designed to have a compact string representation using 16 base-62 characters.
Base 62 comprises only English alphabetic characters and numerals (i.e. `[0-9A-Za-z]`).

Example string values are: `XDC9uT4O0PS5q1O5`, `me1KOWmWGjqv5XHa`, `W4n9Sj84Tv4f81HQ`,
`quayf5S8Xua9Xnqw`.

Each ID has 15 characters of random data and one check character, derived from the first 15.

This provides an approximately 90-bit random space. (For comparison, UUIDs are
random in 128-bit space.)

## Design goals

- Compact string representation using only letters and numbers.
- Sufficient random space to be unique enough for most uses.
- Validity check to catch guessing attempts without needing to query existing values in a database (for example, if
  used to generate links like `https://example.com/l/4jfPi9y19fyv1O0n`).

## Current implementations

- [`LocalId` class in C#](./csharp)
- [`LocalId` class in Kotlin](./kotlin)
- [`LocalId` module in JavaScript](./javascript)
