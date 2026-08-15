using System;
using System.Threading;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class DebugExtensionsTests
{
    public record SampleDto(string Name, int Age);

    [Fact]
    public void Dump_ReturnsIndentedJson_ForObject()
    {
        var dto = new SampleDto("Ali", 30);
        var dump = dto.Dump();

        Assert.Contains("Ali", dump);
        Assert.Contains("30", dump);
        Assert.Contains("\n", dump); // indented => multi-line
    }

    [Fact]
    public void Dump_ReturnsNullLiteral_WhenSourceIsNull()
    {
        object? source = null;
        Assert.Equal("null", source.Dump());
    }

    [Fact]
    public void ToJson_SerializesObjectToCompactJson()
    {
        var dto = new SampleDto("Reza", 25);
        var json = dto.ToJson();

        Assert.Equal("{\"Name\":\"Reza\",\"Age\":25}", json);
    }

    [Fact]
    public void FromJson_DeserializesJsonToObject()
    {
        var json = "{\"Name\":\"Sara\",\"Age\":22}";
        var dto = json.FromJson<SampleDto>();

        Assert.NotNull(dto);
        Assert.Equal("Sara", dto!.Name);
        Assert.Equal(22, dto.Age);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void FromJson_Throws_WhenEmpty(string? json)
    {
        Assert.Throws<ArgumentNullException>(() => json!.FromJson<SampleDto>());
    }

    [Fact]
    public void Measure_Action_ReturnsNonNegativeElapsed()
    {
        var ran = false;
        Action action = () => { Thread.Sleep(5); ran = true; };

        var elapsed = action.Measure();

        Assert.True(ran);
        Assert.True(elapsed >= TimeSpan.Zero);
    }

    [Fact]
    public void Measure_Func_ReturnsResultAndNonNegativeElapsed()
    {
        Func<int> func = () => { Thread.Sleep(5); return 42; };

        var (result, elapsed) = func.Measure();

        Assert.Equal(42, result);
        Assert.True(elapsed >= TimeSpan.Zero);
    }

    [Fact]
    public void Measure_Throws_WhenActionNull()
    {
        Action? action = null;
        Assert.Throws<ArgumentNullException>(() => action!.Measure());
    }
}
