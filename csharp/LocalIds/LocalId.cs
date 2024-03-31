using System.Text;

namespace LocalIds;

/// <summary>
/// <para>
/// A random ID that is very highly likely to be locally unique.
/// </para>
/// <para>
/// Its string representation is 16 characters long in base 62,
/// using only <c>0</c> to <c>9</c>, <c>A</c> to <c>Z</c>
/// and <c>a</c> to <c>z</c>. This corresponds to approximately
/// 90 bits of information (cf. 128 bits for a GUID).
///</para>
/// <para>
/// The first 15 characters are random and the 16th is a check
/// value, derived from the first 15. Examples are <c>"r0Se8CPG1W4jGji3"</c>
/// and <c>"Tv0CmWbK4PH5yyrW"</c>.
/// </para>
/// <para>
/// To create a <c>LocalId</c>:
/// <code>
///     var id = LocalId.NewId();
/// </code>
/// You can specify a random number seed with:
/// <code>
///     var id = LocalId.NewId(seed);
/// </code>
/// </para>
/// </summary>
public class LocalId
{
    public const int CharacterCount = 16;
    private const byte SixtyTwo = 0x3D;

    private readonly string _stringId;

    private LocalId(Random rnd)
    {
        var bytes = new byte[CharacterCount - 1];
        rnd.NextBytes(bytes);
        _stringId = AsBase62(bytes);
    }

    public static LocalId NewId() => new(new Random());

    public static LocalId NewId(int randomSeed) => new(new Random(randomSeed));

    public override string ToString() => _stringId;

    /// <summary>
    /// Validate if a string is a valid <c>LocalId</c>.
    /// </summary>
    /// <param name="idString"></param>
    /// <returns></returns>
    public static bool IsValid(string idString)
    {
        if (idString.Length != CharacterCount) return false;

        var sum = 0;
        for (var idx = 0; idx < idString.Length - 1; ++idx)
        {
            var idx62 = Array.FindIndex(SixtyTwoChars, c => c == idString[idx]);
            if (idx62 == -1)
            {
                return false;
            }

            sum += idx62;
        }

        return idString.Last() == SixtyTwoChars[sum % SixtyTwo];
    }

    public override int GetHashCode()
    {
        return _stringId.GetHashCode();
    }

    public override bool Equals(object? obj) => Equals(obj as LocalId);

    public static bool operator ==(LocalId? lhs, LocalId? rhs)
    {
        if (lhs is not null) return lhs.Equals(rhs);
        return rhs is null;
    }

    public static bool operator !=(LocalId? lhs, LocalId? rhs) => !(lhs == rhs);

    private bool Equals(LocalId? other)
    {
        return other is not null &&
               (ReferenceEquals(this, other) || _stringId.Equals(other._stringId));
    }

    private static string AsBase62(IEnumerable<byte> bytes)
    {
        var builder = new StringBuilder(CharacterCount);

        var sum = 0;
        foreach (var b in bytes)
        {
            var max62 = b & SixtyTwo;
            builder.Append(SixtyTwoChars[max62]);
            sum += max62;
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