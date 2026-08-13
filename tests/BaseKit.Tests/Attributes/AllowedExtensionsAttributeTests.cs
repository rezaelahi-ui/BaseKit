using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class AllowedExtensionsAttributeTests
{
    private class FakeFormFile
    {
        public string FileName { get; set; } = string.Empty;
    }

    [Theory]
    [InlineData("photo.jpg", true)]
    [InlineData("photo.PNG", true)]
    [InlineData("document.pdf", false)]
    public void IsValid_ChecksExtensionCaseInsensitive(string fileName, bool expected)
    {
        var attr = new AllowedExtensionsAttribute("jpg", ".png");
        Assert.Equal(expected, attr.IsValid(fileName));
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenValueIsNull()
    {
        var attr = new AllowedExtensionsAttribute("jpg");
        Assert.True(attr.IsValid(null));
    }

    [Fact]
    public void IsValid_SupportsDuckTypedFileNameProperty()
    {
        var attr = new AllowedExtensionsAttribute("jpg");
        var file = new FakeFormFile { FileName = "photo.jpg" };

        Assert.True(attr.IsValid(file));
    }

    [Fact]
    public void ErrorMessage_ListsAllowedExtensions()
    {
        var attr = new AllowedExtensionsAttribute("jpg", "png");
        Assert.Contains(".jpg", attr.ErrorMessage);
        Assert.Contains(".png", attr.ErrorMessage);
    }
}
