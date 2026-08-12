using System.Net;
using System.Threading.Tasks;
using BaseKit.Extensions;

namespace BaseKit.Tests;

public class IPExtensionsTests
{
    [Fact]
    public async Task Ping_ReturnsTrue_ForLocalhost()
    {
        var result = await IPAddress.Loopback.Ping();
        Assert.True(result);
    }
}
