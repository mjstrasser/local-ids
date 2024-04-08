package localids

fun main() {
    repeat(1000) {
        println(LocalId.newId())
    }
}