using System;
using System.IO;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class FileExtensionsTests
{
    [Fact]
    public void EnsureDirectoryExists_CreatesDirectory_WhenMissing()
    {
        var path = Path.Combine(Path.GetTempPath(), "BaseKitTests_" + Guid.NewGuid());
        try
        {
            Assert.False(Directory.Exists(path));

            var result = path.EnsureDirectoryExists();

            Assert.Equal(path, result);
            Assert.True(Directory.Exists(path));
        }
        finally
        {
            if (Directory.Exists(path)) Directory.Delete(path);
        }
    }

    [Fact]
    public void EnsureDirectoryExists_DoesNothing_WhenAlreadyExists()
    {
        var path = Path.GetTempPath();
        var result = path.EnsureDirectoryExists();

        Assert.Equal(path, result);
        Assert.True(Directory.Exists(path));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void EnsureDirectoryExists_Throws_WhenEmpty(string? path)
    {
        Assert.Throws<ArgumentNullException>(() => path!.EnsureDirectoryExists());
    }

    [Theory]
    [InlineData("report:2024/06.pdf", "report202406.pdf")]
    [InlineData("valid-name.txt", "valid-name.txt")]
    public void GetSafeFileName_RemovesInvalidChars(string input, string expected)
    {
        Assert.Equal(expected, input.GetSafeFileName());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetSafeFileName_Throws_WhenEmpty(string? input)
    {
        Assert.Throws<ArgumentNullException>(() => input!.GetSafeFileName());
    }
}
