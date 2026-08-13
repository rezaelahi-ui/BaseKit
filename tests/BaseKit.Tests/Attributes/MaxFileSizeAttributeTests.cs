using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class MaxFileSizeAttributeTests
{
    private class FakeFormFile
    {
        public long Length { get; set; }
    }

    [Theory]
    [InlineData(500L, 1000L, true)]
    [InlineData(1000L, 1000L, true)]
    [InlineData(1500L, 1000L, false)]
    public void IsValid_ChecksLongValue(long size, long maxSize, bool expected)
    {
        var attr = new MaxFileSizeAttribute(maxSize);
        Assert.Equal(expected, attr.IsValid(size));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsNull()
    {
        var attr = new MaxFileSizeAttribute(1000);
        Assert.True(attr.IsValid(null));
    }

    [Fact]
    public void IsValid_SupportsDuckTypedLengthProperty()
    {
        var attr = new MaxFileSizeAttribute(1000);
        var file = new FakeFormFile { Length = 500 };

        Assert.True(attr.IsValid(file));
    }

    [Fact]
    public void IsValid_ReturnsFalse_WhenDuckTypedLengthExceedsMax()
    {
        var attr = new MaxFileSizeAttribute(1000);
        var file = new FakeFormFile { Length = 1500 };

        Assert.False(attr.IsValid(file));
    }
}
