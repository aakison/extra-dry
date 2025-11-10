using AngleSharp.Html.Dom;
using ExtraDry.Blazor.Forms;
using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExtraDry.Blazor.Tests.Components.Forms;

public class DryInputNumericTests
{
    [Fact]
    public void NumericInputBasicRenderingCorrectness()
    {
        using var context = new TestContext();
        var model = new BasicNumericModel();
        var description = new DecoratorInfo(model.GetType(), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(BasicNumericModel.Numeric));

        var fragment = context.RenderComponent<DryInputNumeric<BasicNumericModel>>(
            (nameof(DryInputBase<BasicNumericModel>.Model), model),
            (nameof(DryInputBase<BasicNumericModel>.Property), property)
            );

        HtmlAssert.TagHasClass(fragment, "div", "input");
        // Inner input is a text field, not numeric
        var input = fragment.Find("input") as IHtmlInputElement;
        Assert.NotNull(input);
        Assert.Equal("text", input.Type);
        // No icons are present by default
        Assert.Throws<ElementNotFoundException>(() => fragment.Find("i"));
        Assert.Throws<ElementNotFoundException>(() => fragment.Find("img"));
        HtmlAssert.TagAttributeValue(fragment, "input", "class", "value");
        HtmlAssert.TagNoAttribute(fragment, "input", "readonly");
    }

    //[Fact]
    //public void NumericInputCssClassPassthrough()
    //{
    //    using var context = new TestContext();
    //    var model = new BasicNumericModel();
    //    var description = new DecoratorInfo(model.GetType(), model);
    //    var property = description.FormProperties.First(e => e.Property.Name == nameof(BasicNumericModel.Numeric));

    //    var fragment = context.RenderComponent<DryInputNumeric<BasicNumericModel>>(
    //        (nameof(DryInputBase<BasicNumericModel>.Model), model),
    //        (nameof(DryInputBase<BasicNumericModel>.Property), property),
    //        (nameof(DryInputBase<BasicNumericModel>.CssClass), "custom-class")
    //        );

    //    // Css class on outer div
    //    var outer = fragment.Find("div") as IHtmlDivElement;
    //    Assert.NotNull(outer);
    //    Assert.Contains("custom-class", outer.ClassList);
    //    // And not in input element
    //    var input = fragment.Find("input") as IHtmlInputElement;
    //    Assert.NotNull(input);
    //    Assert.DoesNotContain("custom-class", input.ClassList);
    //}

    [Fact]
    public void NumericInputMobileKeyboard()
    {
        using var context = new TestContext();
        var model = new BasicNumericModel();
        var description = new DecoratorInfo(model.GetType(), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(BasicNumericModel.Numeric));

        var fragment = context.RenderComponent<DryInputNumeric<BasicNumericModel>>(
            (nameof(DryInputBase<BasicNumericModel>.Model), model),
            (nameof(DryInputBase<BasicNumericModel>.Property), property)
            );

        HtmlAssert.TagAttributeValue(fragment, "input", "pattern", @"\d*");
        HtmlAssert.TagAttributeValue(fragment, "input", "inputmode", @"numeric");
    }

    [Fact]
    public void NumericInputAriaPassthrough()
    {
        using var context = new TestContext();
        var model = new BasicNumericModel();
        var description = new DecoratorInfo(model.GetType(), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(BasicNumericModel.Numeric));

        var fragment = context.RenderComponent<DryInputNumeric<BasicNumericModel>>(
            (nameof(DryInputBase<BasicNumericModel>.Model), model),
            (nameof(DryInputBase<BasicNumericModel>.Property), property),
            ("aria-attribute", "custom-value")
            );

        HtmlAssert.TagAttributeValue(fragment, "div", "aria-attribute", "custom-value");
        HtmlAssert.TagNoAttribute(fragment, "input", "aria-attribute");
    }

    [Theory]
    [InlineData(0.0, "0.00")]
    [InlineData(1.0, "1.00")]
    [InlineData(1.0001, "1.00")]
    [InlineData(0.9999, "1.00")]
    [InlineData(1.005, "1.01")]
    [InlineData(1.0049, "1.00")]
    [InlineData(null, "empty")]
    public void NullableDecimalShowsProperValue(double? value, string expected)
    {
        using var context = new TestContext();
        var model = new NullNumericModel() { Numeric = (decimal?)value };
        var description = new DecoratorInfo(model.GetType(), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(NullNumericModel.Numeric));

        var fragment = context.RenderComponent<DryInputNumeric<NullNumericModel>>(
            (nameof(DryInputBase<NullNumericModel>.Model), model),
            (nameof(DryInputBase<NullNumericModel>.Property), property)
            );

        HtmlAssert.TagAttributeValue(fragment, "input", "placeholder", "Enter a number");
        HtmlAssert.TagAttributeValue(fragment, "input", "value", expected);
    }

    [Fact]
    public void NumericInputExplicitIcon()
    {
        using var context = new TestContext();
        var model = new NullNumericModel();
        var description = new DecoratorInfo(model.GetType(), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(NullNumericModel.Numeric));

        var fragment = context.RenderComponent<DryInputNumeric<NullNumericModel>>(
            (nameof(DryInputBase<NullNumericModel>.Model), model),
            (nameof(DryInputBase<NullNumericModel>.Property), property)
            );

        var icon = fragment.Find("img") as IHtmlImageElement;
        Assert.NotNull(icon);
        Assert.Contains("icon.svg", icon.Source);
        var affordance = fragment.FindAll("img").Skip(1).First() as IHtmlImageElement;
        Assert.NotNull(affordance);
        Assert.Contains("affordance.svg", affordance.Source);
    }

    [Fact]
    public void NumericInputReadOnly()
    {
        using var context = new TestContext();
        var model = new NullNumericModel();
        var description = new DecoratorInfo(model.GetType(), model);
        var property = description.FormProperties.First(e => e.Property.Name == nameof(NullNumericModel.Numeric));

        var fragment = context.RenderComponent<DryInputNumeric<NullNumericModel>>(
            (nameof(DryInputBase<NullNumericModel>.Model), model),
            (nameof(DryInputBase<NullNumericModel>.Property), property),
            (nameof(DryInputNumeric<NullNumericModel>.ReadOnly), true)
            );

        // classes and attributers
        HtmlAssert.TagHasClass(fragment, "div", "readonly");
        HtmlAssert.TagAttribute(fragment, "input", "readonly");
        // suppress affordance
        var affordance = fragment.FindAll("img").Skip(1).FirstOrDefault() as IHtmlImageElement;
        Assert.Null(affordance);
    }

    public class BasicNumericModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        public decimal Numeric { get; set; }
    }

    public class NullNumericModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Display(Prompt = "Enter a number")]
        [DisplayFormat(NullDisplayText = "empty")]
        [InputField(Icon = "icon", Affordance = "affordance")]
        public decimal? Numeric { get; set; }
    }
}
