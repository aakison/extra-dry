using AngleSharp.Html.Dom;
using ExtraDry.Blazor.Forms;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExtraDry.Blazor.Tests.Components;

public class DryInputTextTests {

    [Fact]
    public void StaticComponents()
    {
        using var context = new TestContext();
        var model = new Model { Id = 1, Name = "TheModel" };
        var description = new ViewModelDescription(typeof(Model), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(Model.Name));
        
        var fragment = context.RenderComponent<DryInputText<Model>>(
            (nameof(DryInputText<Model>.Model), model),
            (nameof(DryInputText<Model>.Property), property)
            );

        var input = fragment.Nodes.First() as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("INPUT", input.NodeName);
        Assert.Equal(model.Name, input.Value);
        Assert.Equal("text", input.Type);
    }

    [Fact]
    public void UnmatchedAttributesPassthrough()
    {
        using var context = new TestContext();
        var attributes = new Dictionary<string, object>() {
            { "data-foo", "bar" },
        };

        var fragment = context.RenderComponent<DryInputText<Model>>(
            (nameof(DryInputText<Model>.UnmatchedAttributes), attributes)
            );

        var input = fragment.Nodes.First() as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("bar", input.GetAttribute("data-foo"));
    }

    //[Fact]
    //public void GravatarImgClass()
    //{
    //    var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail));

    //    var img = fragment.Find("img");

    //    Assert.NotNull(img);
    //    Assert.Equal("gravatar", img.ClassName);
    //}

    //[Fact]
    //public void GravatarUsesEmailInAlt()
    //{
    //    var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail));

    //    var img = fragment.Find("img");
    //    var alt = img?.Attributes["alt"]?.Value;

    //    Assert.NotNull(img);
    //    Assert.NotNull(alt);
    //    Assert.Equal(exampleEmail, alt);
    //}

    //[Fact]
    //public void GravatarSuppressesEmailInAlt()
    //{
    //    var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail), ("HideEmail", true));

    //    var img = fragment.Find("img");
    //    var alt = img?.Attributes["alt"]?.Value;

    //    Assert.NotNull(img);
    //    Assert.NotNull(alt);
    //    Assert.NotEqual(exampleEmail, alt);
    //    Assert.Contains("Gravatar", alt);
    //}

    //[Fact]
    //public void GravatarImageHasCorrectHash()
    //{
    //    var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail));

    //    var img = fragment.Find("img");
    //    var src = img?.Attributes["src"]?.Value;

    //    Assert.NotNull(img);
    //    Assert.NotNull(src);
    //    Assert.Contains(exampleHash, src);
    //    Assert.StartsWith("https://www.gravatar.com/avatar/", src);
    //}

    //[Fact]
    //public void GravatarImageRequestsSize()
    //{
    //    var fragment = context.RenderComponent<Gravatar>(("Email", exampleEmail), ("Size", 123));

    //    var img = fragment.Find("img");
    //    var src = img?.Attributes["src"]?.Value;

    //    Assert.NotNull(img);
    //    Assert.NotNull(src);
    //    Assert.Contains("s=123", src);
    //}

    public class Model
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }


}
