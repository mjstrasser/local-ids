# `LocalId`: a local ID in Kotlin

## Create a `LocalId`

To create a `LocalId`:

```kotlin
    // Random ID.
    val randomId = LocalId.newId()
    
    // Random ID, supplying a seed (for testing).
    val seededId = LocalId.newId(randomSeed)
```

## Check validity

The static method `LocalId.isValid()` returns `false` if the string is
the wrong length, contains invalid characters, or if the last character is not a correct
check on the preceding 15.

```kotlin
    LocalId.isValid("n9Gmv0mfDWuy08DG") // true
    LocalId.isValid("n9Gmv0mfDWuy08DH") // false
```
