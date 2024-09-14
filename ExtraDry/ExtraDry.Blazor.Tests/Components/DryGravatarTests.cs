namespace ExtraDry.Blazor.Tests.Components;

public class GravatarTests : IDisposable {

    [Fact]
    public void GravatarHasSurroundingDiv()
    {
        var fragment = context.RenderComponent<Gravatar>();

        var div = fragment.Find("div");
        var img = div.FirstChild;

        Assert.NotNull(div);
        Assert.Equal("gravatar", div.ClassName);
        Assert.NotNull(img);
    }

    [Fact]
    public void GravatarImgClass()
    {
        var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail));

        var img = fragment.Find("img");
            
        Assert.NotNull(img);
        Assert.Equal("gravatar", img.ClassName);
    }

    [Fact]
    public void GravatarUsesEmailInAlt()
    {
        var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail));

        var img = fragment.Find("img");
        var alt = img?.Attributes["alt"]?.Value;

        Assert.NotNull(img);
        Assert.NotNull(alt);
        Assert.Equal(exampleEmail, alt);
    }

    [Fact]
    public void GravatarSuppressesEmailInAlt()
    {
        var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail), ("HideEmail", true));

        var img = fragment.Find("img");
        var alt = img?.Attributes["alt"]?.Value;

        Assert.NotNull(img);
        Assert.NotNull(alt);
        Assert.NotEqual(exampleEmail, alt);
        Assert.Contains("Gravatar", alt);
    }

    [Fact]
    public void GravatarImageHasCorrectHash()
    {
        var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail));

        var img = fragment.Find("img");
        var src = img?.Attributes["src"]?.Value;

        Assert.NotNull(img);
        Assert.NotNull(src);
        Assert.Contains(exampleHash, src);
        Assert.StartsWith("https://www.gravatar.com/avatar/", src);
    }

    [Fact]
    public void GravatarImageRequestsSize()
    {
        var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail), ("Size", 123));

        var img = fragment.Find("img");
        var src = img?.Attributes["src"]?.Value;

        Assert.NotNull(img);
        Assert.NotNull(src);
        Assert.Contains("s=123", src);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        context?.Dispose();
    }

    private const string exampleEmail = "user@example.com";
    private const string exampleHash = "b58996c504c5638798eb6b511e6f49af";

    private readonly TestContext context = new();

}
