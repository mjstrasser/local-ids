using System.Diagnostics;
using AutoFixture.Xunit2;
using Shouldly;
using Xunit;

namespace LocalIds.Test;

public class LocalIdTests
{
    [Fact]
    public void NewId_WithKnownSeed_ProducesKnownString()
    {
        LocalId.NewId(12345678).ToString()
            .ShouldBe("HeXHG00KKvHa9vvU");
    }

    [Fact]
    public void NewId_Length_IncludesCheckDigit()
    {
        var idString = LocalId.NewId().ToString();
        idString.Length.ShouldBe(LocalId.CharacterCount);
    }

    [Fact]
    public void NewId_CrudePerformanceTest()
    {
        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < 1000000; i++)
        {
            LocalId.NewId();
        }

        sw.Stop();
        sw.ElapsedMilliseconds.ShouldBeLessThan(400);
    }

    [Theory]
    [InlineData(0x00, '0')]
    [InlineData(0x09, '9')]
    [InlineData(0x0A, 'A')]
    [InlineData(0x23, 'Z')]
    [InlineData(0x24, 'a')]
    [InlineData(0x3D, 'z')]
    [InlineData(0x40, '0')]
    [InlineData(0x49, '9')]
    [InlineData(0x4A, 'A')]
    [InlineData(0x63, 'Z')]
    [InlineData(0x64, 'a')]
    [InlineData(0x7D, 'z')]
    public void LowSixBitsToBase62(byte bite, char car)
    {
        bite.LowSixBitsToBase62().ShouldBe(car);
    }

    [Fact]
    public void IsValid_WithInvalidCharacters_ReturnsFalse()
    {
        LocalId.IsValid("$%*").ShouldBeFalse();
    }

    [Theory]
    [InlineData("mfaPDeiufLqr1aGZ")]
    [InlineData("HKmTDjG8GWCPT8KX")]
    [InlineData("m8WPeKKHiD19q1Po")]
    public void IsValue_WithCorrectCheckCharacter_ReturnsTrue(string stringId)
    {
        LocalId.IsValid(stringId).ShouldBeTrue();
    }

    [Theory]
    [InlineData("mfaPDeiufLqr1aGY")]
    [InlineData("HKmTDjG8GWCPT8KY")]
    [InlineData("m8WPeKKHiD19q1Pq")]
    public void IsValue_WithIncorrectCheckCharacter_ReturnsFalse(string stringId)
    {
        LocalId.IsValid(stringId).ShouldBeFalse();
    }

    [Theory]
    [AutoData]
    public void GetHashCode_ForTwoIdsWithSameSeed_AreEqual(int seed)
    {
        var localId1 = LocalId.NewId(seed);
        var localId2 = LocalId.NewId(seed);

        localId1.GetHashCode().ShouldBe(localId2.GetHashCode());
    }

    [Theory]
    [AutoData]
    public void GetHashCode_ForTwoIdsWithDifferentSeeds_AreNotEqual(int seed1, int seed2)
    {
        var localId1 = LocalId.NewId(seed1);
        var localId2 = LocalId.NewId(seed2);

        localId1.GetHashCode().ShouldNotBe(localId2.GetHashCode());
    }

    [Theory]
    [AutoData]
    public void Equals_ForTwoIdsWithSameSeed_IsTrue(int seed)
    {
        var localId1 = LocalId.NewId(seed);
        var localId2 = LocalId.NewId(seed);

        localId1.Equals(localId2).ShouldBeTrue();
        (localId1 == localId2).ShouldBeTrue();
    }

    [Theory]
    [AutoData]
    public void Equals_ForTwoIdsWithDifferentSeeds_IsFalse(int seed1, int seed2)
    {
        var localId1 = LocalId.NewId(seed1);
        var localId2 = LocalId.NewId(seed2);

        localId1.Equals(localId2).ShouldBeFalse();
        (localId1 == localId2).ShouldBeFalse();
        (localId1 != localId2).ShouldBeTrue();
    }
}