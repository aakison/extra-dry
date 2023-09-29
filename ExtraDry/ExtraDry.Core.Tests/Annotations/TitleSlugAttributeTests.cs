namespace ExtraDry.Core.Tests;

public class TitleSlugAttributeTests {

    [Theory]
    [InlineData("acme")]
    [InlineData("acme-co")]
    [InlineData("abcde-fghij-klmno")]
    [InlineData("a1-suppliers")]        // With number.
    public void ValidTitleSlugs(string slug)
    {
        var titleSlug = new TitleSlugAttribute();
        var isValid = titleSlug.IsValid(slug);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("Acme")]
    [InlineData("Acme-Co")]
    [InlineData("Abcde-Fghij-Klmno")]
    [InlineData("A1-Suppliers")]
    [InlineData("A1_Suppliers")]
    [InlineData("~!.=+?")]  // No other special chars allowed.
    [InlineData("\\")]      // Backslash not allowed.
    [InlineData("/")]       // Forward slash not allowed.
    public void InvalidTitleSlugs(string slug)
    {
        var titleSlug = new TitleSlugAttribute();
        var isValid = titleSlug.IsValid(slug);

        Assert.False(isValid);
    }
}

