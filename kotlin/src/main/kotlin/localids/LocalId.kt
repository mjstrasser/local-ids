package localids

import kotlin.random.Random

private const val SixtyTwo: Int = 0x3d

private val sixtyTwoCharacters: CharArray = charArrayOf(
    '0', '1', '2', '3', '4', '5', '6', '7',
    '8', '9', 'A', 'B', 'C', 'D', 'E', 'F',
    'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
    'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
    'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd',
    'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
    'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
    'u', 'v', 'w', 'x', 'y', 'z',
)

class LocalId private constructor(random: Random) {

    private val stringId: String

    init {
        val bytes = random.nextBytes(CHARACTER_COUNT - 1)
        stringId = asBase62(bytes)
    }

    companion object {
        const val CHARACTER_COUNT: Int = 16

        fun newId(): LocalId = LocalId(Random.Default)
        fun newId(seed: Long): LocalId = LocalId(Random(seed))
    }

    private fun asBase62(bytes: ByteArray): String {
        val builder = StringBuilder()
        var sum = 0
        for (b in bytes) {
            val max62 = b.toInt() and SixtyTwo
            builder.append(sixtyTwoCharacters[max62])
            sum += max62
        }
        builder.append(sixtyTwoCharacters[sum % SixtyTwo])
        return builder.toString()
    }

    override fun toString(): String = stringId
}