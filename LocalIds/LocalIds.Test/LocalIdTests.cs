using System.Diagnostics;
using Shouldly;
using Xunit;

namespace LocalIds.Test;

public class LocalIdTests
{
    [Fact]
    public void NewId_WithKnownSeed_ProducesKnownString()
    {
        LocalId.NewId(12345678).ToString()
            .ShouldBe("JgXJG02KMvHc9xv5V");
    }

    [Fact]
    public void NewId_Length_IncludesCheckDigit()
    {
        var idString = LocalId.NewId().ToString();
        idString.Length.ShouldBe(LocalId.ByteCount + 1);
    }

    [Fact]
    public void NewId_PerformanceTest()
    {
        var sw = new Stopwatch();
        sw.Start();
        for (var i = 0; i < 1000000; i++)
        {
            LocalId.NewId();
        }
        sw.Stop();
        sw.ElapsedMilliseconds.ShouldBeLessThan(300);
    }

    [Theory]
    [InlineData(0x00, '0')]
    [InlineData(0x09, '9')]
    [InlineData(0x0A, 'A')]
    [InlineData(0x23, 'Z')]
    [InlineData(0x24, 'a')]
    [InlineData(0x3D, 'z')]
    [InlineData(0x3E, '-')]
    [InlineData(0x3F, '_')]
    [InlineData(0x40, '0')]
    [InlineData(0x49, '9')]
    [InlineData(0x4A, 'A')]
    [InlineData(0x63, 'Z')]
    [InlineData(0x64, 'a')]
    [InlineData(0x7D, 'z')]
    [InlineData(0x7E, '-')]
    [InlineData(0x7F, '_')]
    public void LowSixBitsToBase64(byte bite, char car)
    {
        bite.LowSixBitsToBase64().ShouldBe(car);
    }
}