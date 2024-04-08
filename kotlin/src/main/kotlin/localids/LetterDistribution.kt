package localids

import kotlin.random.Random
import kotlin.random.nextUBytes

@OptIn(ExperimentalUnsignedTypes::class)
fun main() {
    val random = Random.Default
    val samples = 10_000_000
    val SixtyTwo = 62
    val counts = IntArray(SixtyTwo)
    for (sample in 0 until samples) {
        val byte = random.nextUBytes(1).first().toInt() % SixtyTwo
        counts[byte]++
    }
    for (idx in 0 until SixtyTwo) {
        println("${sixtyTwoCharacters[idx]}: ${counts[idx]}")
    }
}

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