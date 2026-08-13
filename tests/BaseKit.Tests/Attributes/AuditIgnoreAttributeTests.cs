using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class AuditIgnoreAttributeTests
{
    [Fact]
    public void CanBeInstantiated()
    {
        var attr = new AuditIgnoreAttribute();
        Assert.NotNull(attr);
    }
}
