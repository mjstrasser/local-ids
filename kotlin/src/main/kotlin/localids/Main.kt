package localids

fun main() {
    repeat(10) {
        println(LocalId.newId())
    }
}