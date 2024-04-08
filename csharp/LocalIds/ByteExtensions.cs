namespace LocalIds;

/// <summary>
/// Extensions class used by tests of base-62 encoding.
/// </summary>
public static class ByteExtensions
{
    private const byte SixtyTwo = 0x3E;

    public static char LowSixBitsToBase62(this byte bite) =>
        SixtyTwoChars[bite % SixtyTwo];

    private static readonly char[] SixtyTwoChars =
    [
        '0', '1', '2', '3', '4', '5', '6', '7',
        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', // 0x0F
        'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
        'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', // 0x1F
        'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
        'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', // 0x2F
        'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z',
    ];
}