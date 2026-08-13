using BaseKit.Attributes;

namespace BaseKit.Tests.Attributes;

public class NoteAttributeTests
{
    [Fact]
    public void Constructor_SetsSummary()
    {
        var attr = new NoteAttribute("خلاصه متد");
        Assert.Equal("خلاصه متد", attr.Summary);
        Assert.Null(attr.Description);
    }

    [Fact]
    public void Description_CanBeSetViaProperty()
    {
        var attr = new NoteAttribute("خلاصه") { Description = "توضیح کامل" };
        Assert.Equal("توضیح کامل", attr.Description);
    }
}
