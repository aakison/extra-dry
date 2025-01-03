namespace ExtraDry.Blazor.Tests;

internal static class HtmlAssert
{
    internal static void TagAttribute<T>(IRenderedComponent<T> component, string tag, string attribute)
    where T : class, Microsoft.AspNetCore.Components.IComponent
    {
        var element = component.Find(tag);
        Assert.NotNull(element);
        var attr = element.Attributes.First(e => e.Name == attribute);
        Assert.NotNull(attr);
    }

    internal static void TagAttributeValue<T>(IRenderedComponent<T> component, string tag, string attribute, string value)
        where T : class, Microsoft.AspNetCore.Components.IComponent
    {
        var element = component.Find(tag);
        Assert.NotNull(element);
        var attributeValue = element.GetAttribute(attribute);
        Assert.NotNull(attributeValue);
        Assert.Equal(value, attributeValue);
    }

    internal static void TagHasClass<T>(IRenderedComponent<T> component, string tag, string cssClass)
        where T : class, Microsoft.AspNetCore.Components.IComponent
    {
        var element = component.Find(tag);
        Assert.NotNull(element);
        Assert.Contains(cssClass, element.ClassList);
    }

    internal static void TagNoAttribute<T>(IRenderedComponent<T> component, string tag, string attribute)
    where T : class, Microsoft.AspNetCore.Components.IComponent
    {
        var element = component.Find(tag);
        Assert.NotNull(element);
        var count = element.Attributes.Count(e => e.Name == attribute);
        Assert.Equal(0, count);
    }
}
