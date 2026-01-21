package localids

import io.kotest.assertions.fail
import io.kotest.core.spec.style.DescribeSpec
import io.kotest.matchers.comparables.shouldBeLessThan
import io.kotest.matchers.shouldBe
import io.kotest.property.Arb
import io.kotest.property.arbitrary.long
import io.kotest.property.forAll
import kotlin.io.path.Path
import kotlin.time.measureTimedValue

class LocalIdTest : DescribeSpec({

    describe("LocalId.newId()") {
        it("length includes check digit") {
            LocalId.newId().toString().length shouldBe LocalId.CHARACTER_COUNT
        }
        it("with known seed produces known string") {
            LocalId.newId(123456789).toString() shouldBe "4W7gpjVhDs3OMytq"
        }
        it("crude performance test") {
            val time = measureTimedValue {
                repeat(1_000_000) { LocalId.newId() }
            }
            time.duration.inWholeMilliseconds shouldBeLessThan 400
        }
    }

    describe("LocalId.isValid()") {
        it("returns false if the string is the wrong length") {
            LocalId.isValid("0123456789ABCDEFG") shouldBe false
            LocalId.isValid("0123456789ABCDE") shouldBe false
        }
        it("returns false from invalid characters") {
            LocalId.isValid("0123456789ABCDE?") shouldBe false
        }
        it("returns true if the check character is correct") {
            LocalId.isValid("0000000000000000") shouldBe true
            LocalId.isValid("A00000000000000A") shouldBe true
            LocalId.isValid("000b00000000000b") shouldBe true
            LocalId.isValid("000000000X00000X") shouldBe true
            LocalId.isValid("0123456789ABCDEh") shouldBe true
        }
        it("returns false if the check character is incorrect") {
            LocalId.isValid("0123456789ABCDEj") shouldBe false
        }
        it("returns true for many values from a file") {
            val gradleUserDir = System.getProperty("user.dir")
            val ids = gradleUserDir?.let { userDir ->
                Path(userDir, "../valid-ids.txt")
                    .toFile()
                    .readLines()
            }
            if (ids == null) fail("valid-ids.txt not found")
            for (id in ids) {
                LocalId.isValid(id) shouldBe true
            }
        }
    }

    describe("LocalId.hashCode()") {
        it("for two IDs with the same seed are equal") {
            forAll<Long> { seed ->
                LocalId.newId(seed).hashCode() == LocalId.newId(seed).hashCode()
            }
        }
        it("for two IDs with different seeds are not equal") {
            forAll(Arb.long(), Arb.long()) { seed1, seed2 ->
                seed1 == seed2 || LocalId.newId(seed1).hashCode() != LocalId.newId(seed2).hashCode()
            }
        }
    }

    describe("LocalId.equals()") {
        it("for two IDs with the same seed are equal") {
            forAll<Long> { seed ->
                LocalId.newId(seed) == LocalId.newId(seed)
            }
        }
        it("for two IDs with different seeds are not equal") {
            forAll(Arb.long(), Arb.long()) { seed1, seed2 ->
                seed1 == seed2 || LocalId.newId(seed1) != LocalId.newId(seed2)
            }
        }
    }
})
