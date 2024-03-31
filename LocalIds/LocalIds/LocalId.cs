using System.Text;

namespace LocalIds;

public class LocalId
{
    public const int ByteCount = 16;
    private const byte SixtyTwo = 0x3D;

    private readonly byte[] _bytes;

    private LocalId(Random rnd)
    {
        _bytes = new byte[ByteCount];
        rnd.NextBytes(_bytes);
    }

    public static LocalId NewId() => new(new Random());

    public static LocalId NewId(int randomSeed) => new(new Random(randomSeed));

    public override string ToString() => AsBase64();

    private string AsBase64()
    {
        var builder = new StringBuilder(ByteCount + 1);

        var sum = 0;
        foreach (var b in _bytes)
        {
            var sixBits = b & SixtyTwo;
            builder.Append(SixtyTwoChars[sixBits]);
            sum += sixBits;
        }

        builder.Append(SixtyTwoChars[sum % SixtyTwo]);
        return builder.ToString();
    }

    private static readonly char[] SixtyTwoChars =
    [
        '0', '1', '2', '3', '4', '5', '6', '7',
        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
        'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
        'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
        'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
        'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
        'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z',
    ];
}