namespace ExtraDry.Blazor.Tests.Components;

public class DryButtonTests : IDisposable
{
    [Fact]
    public void DryButtonNoCommandAttributeDefaults()
    {
        var commandInfo = new CommandInfo(this, ParameterlessMethod);
        var fragment = context.RenderComponent<DryButton>(("Command", commandInfo));

        var button = fragment.Find("button");
        var span = button.FirstChild;

        Assert.Contains(nameof(CommandContext.Regular).ToLowerInvariant(), button.ClassName);
        Assert.Null(button.Attributes["disabled"]);
        Assert.Equal("Parameterless Method", span?.TextContent); // Note spacing introduced.
    }

    [Fact]
    public void DryButtonExecutesParameterless()
    {
        var commandInfo = new CommandInfo(this, ParameterlessMethod);
        var fragment = context.RenderComponent<DryButton>(("Command", commandInfo));

        fragment.Find("button").Click();

        Assert.Equal(nameof(ParameterlessMethod), LastMethodClicked);
    }

    private void ParameterlessMethod()
    {
        LastMethodClicked = nameof(ParameterlessMethod);
    }

    [Fact]
    public void DecoratedAttributeDefaults()
    {
        var commandInfo = new CommandInfo(this, DecoratedParameterlessMethod);
        var fragment = context.RenderComponent<DryButton>(("Command", commandInfo));

        var button = fragment.Find("button");
        var span = fragment.Find("span");
        var icon = fragment.Find("img");

        Assert.Contains(nameof(CommandContext.Primary).ToLowerInvariant(), button.ClassName);
        Assert.Null(button.Attributes["disabled"]);
        Assert.Equal("Click Me", span.TextContent);
    }

    [Command(CommandContext.Primary, Icon = "plus", Name = "Click Me")]
    private void DecoratedParameterlessMethod()
    {
        LastMethodClicked = nameof(DecoratedParameterlessMethod);
    }

    [Fact]
    public async Task DryButtonExecutesParameterMethod()
    {
        var commandInfo = new CommandInfo(this, AsyncMethod);
        var fragment = context.RenderComponent<DryButton>(("Command", commandInfo));

        await fragment.Find("button").ClickAsync(new MouseEventArgs());

        Assert.Equal(nameof(AsyncMethod), LastMethodClicked);
    }

    private async Task AsyncMethod()
    {
        await Task.Delay(1); // delay before setting name, ensures it's called with await and not running just sync portion of method.
        LastMethodClicked = nameof(AsyncMethod);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        context?.Dispose();
    }

    private readonly TestContext context = new();

    private string LastMethodClicked { get; set; } = string.Empty;
}
