namespace ExtraDry.Core.Tests;

public class SlugAttributeTests
{

    [Theory]
    [InlineData("acme")]
    [InlineData("acme-co")]
    [InlineData("abcde-fghij-klmno")]
    [InlineData("a1-suppliers")]        // With number.
    public void ValidTitleSlugs(string slug)
    {
        var titleSlug = new SlugAttribute(SlugType.Lowercase);
        var isValid = titleSlug.IsValid(slug);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("Acme")]
    [InlineData("Acme-Co")]
    [InlineData("Abcde-Fghij-Klmno")]
    [InlineData("A1-Suppliers")]
    [InlineData("A1_Suppliers")]
    [InlineData("a_b")]
    [InlineData("a~b")]
    [InlineData("a!b")]
    [InlineData("a.b")]
    [InlineData("a=b")]
    [InlineData("a+b")]
    [InlineData("a?b")]
    [InlineData("a\\b")]
    [InlineData("a/b")]
    public void InvalidTitleSlugs(string slug)
    {
        var titleSlug = new SlugAttribute(SlugType.Lowercase);
        var isValid = titleSlug.IsValid(slug);

        Assert.False(isValid);
    }

    [Theory]
    [InlineData("acme")]
    [InlineData("acme-co")]
    [InlineData("Acme-Co")]
    [InlineData("abcde-fghij-klmno")]
    [InlineData("A1-suppliers")]
    public void ValidCodeSlugs(string slug)
    {
        var titleSlug = new SlugAttribute(SlugType.MixedCase);
        var isValid = titleSlug.IsValid(slug);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("A1_Suppliers")]
    [InlineData("a_b")]
    [InlineData("a~b")]
    [InlineData("a!b")]
    [InlineData("a.b")]
    [InlineData("a=b")]
    [InlineData("a+b")]
    [InlineData("a?b")]
    [InlineData("a\\b")]
    [InlineData("a/b")]
    public void InvalidCodeSlugs(string slug)
    {
        var titleSlug = new SlugAttribute(SlugType.MixedCase);
        var isValid = titleSlug.IsValid(slug);

        Assert.False(isValid);
    }

}

