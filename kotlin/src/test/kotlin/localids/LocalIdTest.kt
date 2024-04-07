package localids

import io.kotest.core.spec.style.DescribeSpec
import io.kotest.matchers.comparables.shouldBeLessThan
import io.kotest.matchers.shouldBe
import kotlin.time.measureTimedValue

class LocalIdTest : DescribeSpec({

    describe("LocalId.newId()") {
        it("length includes check digit") {
            LocalId.newId().toString().length shouldBe LocalId.CHARACTER_COUNT
        }
        it("with known seed produces known string") {
            LocalId.newId(123456789).toString() shouldBe "yS1ajfPfDm1KKur0"
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
            LocalId.isValid("OW0Pu5HTzjLDCWjp") shouldBe true
            LocalId.isValid("fPqrufrjTnHunOfL") shouldBe true
        }
        it("returns false if the check character is incorrect") {
            LocalId.isValid("OW0Pu5HTzjLDCWjP") shouldBe false
            LocalId.isValid("fPqrufrjTnHunOfq") shouldBe false
        }
    }
})
