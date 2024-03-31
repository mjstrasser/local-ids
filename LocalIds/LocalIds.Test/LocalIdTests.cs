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
    public void IsValid_WithInvalidChar_ReturnsFalse()
    {
        LocalId.IsValid("$%*").ShouldBeFalse();
    }

    [Fact]
    public void IsValue_WithCorrectCheck_ReturnsTrue()
    {
        LocalId.IsValid("HeXHG00KKvHa9vvU").ShouldBeTrue();
    }

    [Fact]
    public void IsValue_WithIncorrectCheck_ReturnsFalse()
    {
        LocalId.IsValid("HeXHG00KKvHa9vvX").ShouldBeFalse();
    }

    [Theory]
    [AutoData]
    public void GetHashCode_ForTwoIdsWithSameSeed_AreEqual(int seed)
    {
        LocalId.NewId(seed).GetHashCode()
            .ShouldBe(LocalId.NewId(seed).GetHashCode());
    }

    [Theory]
    [AutoData]
    public void GetHashCode_ForTwoIdsWithDifferentSeeds_AreNotEqual(int seed1, int seed2)
    {
        LocalId.NewId(seed1).GetHashCode()
            .ShouldNotBe(LocalId.NewId(seed2).GetHashCode());
    }

    [Theory]
    [AutoData]
    public void Equals_ForTwoIdsWithSameSeed_IsTrue(int seed)
    {
        LocalId.NewId(seed).Equals(LocalId.NewId(seed)).ShouldBeTrue();
        (LocalId.NewId(seed) == LocalId.NewId(seed)).ShouldBeTrue();
    }

    [Theory]
    [AutoData]
    public void Equals_ForTwoIdsWithDifferentSeeds_IsFalse(int seed1, int seed2)
    {
        LocalId.NewId(seed1).Equals(LocalId.NewId(seed2)).ShouldBeFalse();
        (LocalId.NewId(seed1) == LocalId.NewId(seed2)).ShouldBeFalse();
        (LocalId.NewId(seed1) != LocalId.NewId(seed2)).ShouldBeTrue();
    }
}