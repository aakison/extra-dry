namespace ExtraDry.Core.Tests;

public class CodeSlugAttributeTests {

    [Theory]
    [InlineData("acme")]
    [InlineData("acme-co")]
    [InlineData("Acme-Co")]
    [InlineData("abcde-fghij-klmno")]
    [InlineData("A1-suppliers")]
    public void ValidCodeSlugs(string slug)
    {
        var titleSlug = new CodeSlugAttribute();
        var isValid = titleSlug.IsValid(slug);

        Assert.True(isValid);
    }

    [Theory]
    [InlineData("A1_Suppliers")]
    [InlineData("~!.=+?")]  // No other special chars allowed.
    [InlineData("\\")]      // Backslash not allowed.
    [InlineData("/")]       // Forward slash not allowed.
    public void InvalidCodeSlugs(string slug)
    {
        var titleSlug = new CodeSlugAttribute();
        var isValid = titleSlug.IsValid(slug);

        Assert.False(isValid);
    }
}

