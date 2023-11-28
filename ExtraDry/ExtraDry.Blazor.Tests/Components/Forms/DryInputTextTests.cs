using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using ExtraDry.Blazor.Forms;
using Microsoft.AspNetCore.Components;
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
    public void ReadOnlyComponents()
    {
        using var context = new TestContext();
        var model = new Model { Id = 1, Name = "TheModel" };
        var description = new ViewModelDescription(typeof(Model), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(Model.Name));

        var fragment = context.RenderComponent<DryInputText<Model>>(
            (nameof(DryInputText<Model>.Model), model),
            (nameof(DryInputText<Model>.Property), property),
            (nameof(DryInputText<Model>.EditMode), EditMode.ReadOnly)
            );

        var input = fragment.Find("input");
        Assert.NotNull(input);
        Assert.True(input.IsReadOnly());
        //Assert.Contains("readonly", input.ClassName);
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

    [Fact]
    public void ChangeEventSync()
    {
        using var context = new TestContext();
        var model = new Model { Id = 1, Name = "TheModel" };
        var description = new ViewModelDescription(typeof(Model), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(Model.Name));
        var fragment = context.RenderComponent<DryInputText<Model>>(
            (nameof(DryInputText<Model>.Model), model),
            (nameof(DryInputText<Model>.Property), property)
            );
        var input = fragment.Find("input");
        var newValue = "Updated";
        var args = new ChangeEventArgs { Value = newValue };

        input.NodeValue = newValue;
        input.Change(args);

        Assert.NotNull(input);
        Assert.Equal(newValue, model.Name);
    }

    [Fact]
    public async Task ChangeEventAsync()
    {
        using var context = new TestContext();
        var model = new Model { Id = 1, Name = "TheModel" };
        var description = new ViewModelDescription(typeof(Model), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(Model.Name));
        var fragment = context.RenderComponent<DryInputText<Model>>(
            (nameof(DryInputText<Model>.Model), model),
            (nameof(DryInputText<Model>.Property), property)
            );
        var input = fragment.Find("input");
        var newValue = "Updated";
        var args = new ChangeEventArgs { Value = newValue };

        input.NodeValue = newValue;
        await input.ChangeAsync(args);

        Assert.NotNull(input);
        Assert.Equal(newValue, model.Name);
    }


    [Fact]
    public void ModellessChangeEventAsync()
    {
        using var context = new TestContext();
        var model = new Model { Id = 1, Name = "TheModel" };
        var fragment = context.RenderComponent<DryInputText<Model>>(
            (nameof(DryInputText<Model>.Model), model)
            );
        var input = fragment.Find("input");
        var newValue = "Updated";
        var args = new ChangeEventArgs { Value = newValue };

        input.NodeValue = newValue;
        input.Change(args);

        Assert.NotNull(input);
        Assert.Equal("TheModel", model.Name); // i.e. no change.
    }

    public class Model
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

    }


}
