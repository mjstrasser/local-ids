using System.Diagnostics;
using AutoFixture.Xunit2;
using Shouldly;
using Xunit;

namespace LocalIds.Test;

public class LocalIdTests
{
    [Fact]
    public void NewId_Length_IncludesCheckDigit()
    {
        var idString = LocalId.NewId().ToString();
        idString.Length.ShouldBe(LocalId.CharacterCount);
    }

    [Fact]
    public void NewId_WithKnownSeed_ProducesKnownString()
    {
        LocalId.NewId(12345678).ToString()
            .ShouldBe("LkZNK44OOvHcDzvA");
    }

    [Fact]
    public void NewId_CrudePerformanceTest()
    {
        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < 10000; i++)
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
    [InlineData(0x40, '2')]
    [InlineData(0x49, 'B')]
    [InlineData(0x4A, 'C')]
    [InlineData(0x63, 'b')]
    [InlineData(0x64, 'c')]
    [InlineData(0x7D, '1')]
    public void LowSixBitsToBase62(byte bite, char ch)
    {
        bite.LowSixBitsToBase62().ShouldBe(ch);
    }

    [Theory]
    [InlineData("0123456789ABCDE")]
    [InlineData("0123456789ABCDEFG")]
    public void IsValid_WithWrongNumberOfCharacters_ReturnsFalse(string stringId)
    {
        LocalId.IsValid(stringId).ShouldBeFalse();
    }

    [Fact]
    public void IsValid_WithInvalidCharacters_ReturnsFalse()
    {
        LocalId.IsValid("0123456789ABCDE?").ShouldBeFalse();
    }

    [Theory]
    [InlineData("0000000000000000")]
    [InlineData("A00000000000000A")]
    [InlineData("000b00000000000b")]
    [InlineData("000000000X00000X")]
    [InlineData("0123456789ABCDEh")]
    public void IsValid_WithCorrectCheckCharacter_ReturnsTrue(string stringId)
    {
        LocalId.IsValid(stringId).ShouldBeTrue();
    }

    [Theory]
    [InlineData("000000000000000A")]
    [InlineData("A00000000000000B")]
    [InlineData("000b00000000000c")]
    [InlineData("000000000X00000Y")]
    [InlineData("0123456789ABCDEi")]
    public void IsValid_WithIncorrectCheckCharacter_ReturnsFalse(string stringId)
    {
        LocalId.IsValid(stringId).ShouldBeFalse();
    }

    [Fact]
    public void IsValid_ReadingValidIDsFromFile_ReturnsTrueForAll()
    {
        var ids = File.ReadLines("valid-ids.txt");
        foreach (var id in ids)
        {
            LocalId.IsValid(id).ShouldBeTrue();
        }
    }

    [Theory, AutoData]
    public void GetHashCode_ForTwoIdsWithSameSeed_AreEqual(int seed)
    {
        var localId1 = LocalId.NewId(seed);
        var localId2 = LocalId.NewId(seed);

        localId1.GetHashCode().ShouldBe(localId2.GetHashCode());
    }

    [Theory, AutoData]
    public void GetHashCode_ForTwoIdsWithDifferentSeeds_AreNotEqual(int seed1, int seed2)
    {
        var localId1 = LocalId.NewId(seed1);
        var localId2 = LocalId.NewId(seed2);

        localId1.GetHashCode().ShouldNotBe(localId2.GetHashCode());
    }

    [Theory, AutoData]
    public void Equals_ForTwoIdsWithSameSeed_IsTrue(int seed)
    {
        var localId1 = LocalId.NewId(seed);
        var localId2 = LocalId.NewId(seed);

        localId1.Equals(localId2).ShouldBeTrue();
        (localId1 == localId2).ShouldBeTrue();
    }

    [Theory, AutoData]
    public void Equals_ForTwoIdsWithDifferentSeeds_IsFalse(int seed1, int seed2)
    {
        var localId1 = LocalId.NewId(seed1);
        var localId2 = LocalId.NewId(seed2);

        localId1.Equals(localId2).ShouldBeFalse();
        (localId1 == localId2).ShouldBeFalse();
        (localId1 != localId2).ShouldBeTrue();
    }
}