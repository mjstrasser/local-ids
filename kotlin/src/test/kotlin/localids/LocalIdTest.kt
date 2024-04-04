package localids

import io.kotest.core.spec.style.DescribeSpec
import io.kotest.matchers.shouldBe

class LocalIdTest : DescribeSpec({

    describe("LocalId.newId()") {
        it("length includes check digit") {
            LocalId.newId().toString().length shouldBe LocalId.CHARACTER_COUNT
        }
        it("with known seed produces known string") {
            LocalId.newId(123456789).toString() shouldBe "yS1ajfPfDm1KKur0"
        }
    }

})
