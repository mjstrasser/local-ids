package localids

import io.kotest.assertions.fail
import io.kotest.core.spec.style.DescribeSpec
import io.kotest.data.forAll
import io.kotest.matchers.comparables.shouldBeLessThan
import io.kotest.matchers.shouldBe
import io.kotest.matchers.shouldNotBe
import kotlin.io.path.Path
import kotlin.time.measureTimedValue

class LocalIdTest : DescribeSpec({

    describe("LocalId.newId()") {
        it("length includes check digit") {
            LocalId.newId().toString().length shouldBe LocalId.CHARACTER_COUNT
        }
        it("with known seed produces known string") {
            LocalId.newId(123456789).toString() shouldBe "yS1ajfPfDm1KKurs"
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
            LocalId.isValid("yS1ajfPfDm1KKur") shouldBe false
            LocalId.isValid("yS1ajfPfDm1KKur0X") shouldBe false
        }
        it("returns false from invalid characters") {
            LocalId.isValid("y$1ajfPfDm1KKur0") shouldBe false
        }
        it("returns true if the check character is correct") {
            LocalId.isValid("mur9WvSe5HL1XGTB") shouldBe true
            LocalId.isValid("vaObv5f1WeOHvrnY") shouldBe true
        }
        it("returns false if the check character is incorrect") {
            LocalId.isValid("OW0Pu5HTzjLDCWjP") shouldBe false
            LocalId.isValid("fPqrufrjTnHunOfq") shouldBe false
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
                LocalId.newId(seed).hashCode() shouldBe LocalId.newId(seed).hashCode()
            }
        }
        it("for to IDs with different seeds are not equal") {
            forAll<Long, Long> { seed1, seed2 ->
                LocalId.newId(seed1).hashCode() shouldNotBe LocalId.newId(seed2).hashCode()
            }
        }
    }

    describe("LocalId.equals()") {
        it("for two IDs with the same seed are equal") {
            forAll<Long> { seed ->
                LocalId.newId(seed) shouldBe LocalId.newId(seed)
            }
        }
        it("for to IDs with different seeds are not equal") {
            forAll<Long, Long> { seed1, seed2 ->
                LocalId.newId(seed1) shouldNotBe LocalId.newId(seed2)
            }
        }
    }
})
