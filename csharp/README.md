# `LocalId`: a local ID in C#

`LocalId` is designed to have a compact string representation using 16 base-62 characters.
Base 62 comprises only English alphabetic characters and numerals (i.e. `[0-9A-Z-a-z]`).

Example string values are: `XDC9uT4O0PS5q1O5`, `me1KOWmWGjqv5XHa`, `W4n9Sj84Tv4f81HQ`,
`quayf5S8Xua9Xnqw`.

Each ID has 15 characters of random data and one check character, derived from the first 15.

This provides an approximately 90-bit random space. (For comparison, UUIDs or GUIDs are
random in 128-bit space.)

## Create a `LocalId`

To create a `LocalId`:

```csharp
    // Random ID.
    var randomId = LocalId.NewId();
    
    // Random ID, supplying a seed (for testing).
    var seededId = LocalId.NewId(randomSeed);
```

## Check validity

The static method `LocalId.IsValid()` returns `false` if the string is
the wrong length, contains invalid characters, or if the last character is not a correct
check on the preceding 15.

```csharp
    LocalId.IsValid("n9Gmv0mfDWuy08DG") // true
    LocalId.IsValid("n9Gmv0mfDWuy08DH") // false
```

## Design goals

- Compact string representation using only letters and numbers.
- Sufficient random space to be unique enough for most uses.
- Validity check to catch guessing attempts without needing to query existing values in a database (for example, if
  used to generate links like `https://example.com/l/DWK4CvTCWj5zDSC3`).
