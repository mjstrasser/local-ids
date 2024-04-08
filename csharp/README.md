# `LocalId`: a local ID in C#

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
