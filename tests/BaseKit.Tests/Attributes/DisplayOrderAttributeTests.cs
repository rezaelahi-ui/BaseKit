using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class DisplayOrderAttributeTests
{
    [Fact]
    public void Constructor_SetsOrder()
    {
        var attr = new DisplayOrderAttribute(3);
        Assert.Equal(3, attr.Order);
    }
}
