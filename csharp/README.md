# `LocalId`: a local ID in C#

`LocalId` is designed to have a compact string representation using 16 base-62 characters.
Base 62 comprises only English alphabetic characters and numerals (i.e. `[0-9A-Z-a-z]`).

Example string values are: `4Gje9T9KrGK11L4i`, `WLqW9rCvPaS9PHeL`, `r8HT108iS4C1b402`,
`qivnX0jnHGvvS5CX`.

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
    LinkId.IsValid("r8HT108iS4C1b402") // true
    LinkId.IsValid("r8HT108iS4C1b403") // false
```

## Design goals

- Compact string representation using only letters and numbers.
- Sufficient random space to be unique enough for most uses.
- Validity check to catch guessing attempts without needing to query existing values in a database (for example, if
  used to generate links like `https//example.com/l/WLqW9rCvPaS9PHeL`).