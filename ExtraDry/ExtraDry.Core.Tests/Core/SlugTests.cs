namespace ExtraDry.Core.Tests;

public class SlugTests {
    #region Slug Tests
    [Fact]
    public void SlugWithNullArg()
    {
        var slug = Slug.ToSlug(null!);

        Assert.Empty(slug);
    }

    [Fact]
    public void SlugWithTheLotLowercase()
    {
        var slug = Slug.ToSlug("This is a test @6!%4 _- tHINGS!");

        Assert.Equal("this-is-a-test-6-4-_-things", slug);
    }

    [Fact]
    public void SlugWithTheLotNotLowercase()
    {
        var slug = Slug.ToSlug("This is a test @6!%4 _- tHINGS!", false);

        Assert.Equal("This-is-a-test-6-4-_-tHINGS", slug);
    }

    [Fact]
    public void SlugWithTheLotLowercaseAndLimit()
    {
        var slug = Slug.ToSlug("This is a test @6!%4 _- tHINGS!", 5);

        Assert.Equal("this-", slug);
    }

    [Fact]
    public void UniqueSlugWithTheLotNoMatch()
    {
        var slug = Slug.ToUniqueCodeSlug("This is a test @6!%4 _- tHINGS!", 15, new List<string> { "foo", "this-is-a" });

        Assert.Equal("This-is-a", slug);
    }

    [Fact]
    public async Task AsyncUniqueSlugWithTheLotNoMatch()
    {
        var slug = await Slug.ToUniqueSlugAsync("This is a test @6!%4 _- tHINGS!", 15, slug => GetAsyncList(slug, "no-match"));

        Assert.Equal("this-is-a", slug);
    }

    [Fact]
    public async Task AsyncUniqueSlugWithTheLotWithMatch()
    {
        var expected = "This-is-a";
        var slug = await Slug.ToUniqueCodeSlugAsync("This is a test @6!%4 _- tHINGS!", 15, slug => GetAsyncList(slug, expected));

        Assert.NotEqual(expected, slug);
        Assert.StartsWith(expected, slug);
        Assert.Equal(expected.Length + 6, slug.Length);
    }
    #endregion

    #region TitleSlug Tests
    [Fact]
    public void TitleSlugWithTheLot()
    {
        var slug = Slug.ToTitleSlug("This is a test @6!%4 _- tHINGS!");

        Assert.Equal("this-is-a-test-6-4-things", slug);
    }

    [Fact]
    public void TitleSlugWithTheLotAndLimit()
    {
        var slug = Slug.ToTitleSlug("This is a test @6!%4 _- tHINGS!", 5);

        Assert.Equal("this-", slug);
    }

    [Fact]
    public void UniqueTitleSlugWithTheLotNoMatch()
    {
        var slug = Slug.ToUniqueTitleSlug("This is a test @6!%4 _- tHINGS!", 15, new List<string> { "foo", "This-is-a" });

        Assert.Equal("this-is-a", slug);
    }

    [Fact]
    public void UniqueTitleSlugWithTheLotWithMatch()
    {
        var expected = "this-is-a";
        var slug = Slug.ToUniqueTitleSlug("This is a test @6!%4 _- tHINGS!", 15, new List<string> { "foo", expected });

        Assert.NotEqual(expected, slug);
        Assert.StartsWith(expected, slug);
        Assert.Equal(expected.Length + 6, slug.Length);
    }

    [Fact]
    public async Task AsyncUniqueTitleSlugWithTheLotNoMatch()
    {
        var slug = await Slug.ToUniqueTitleSlugAsync("This is a test @6!%4 _- tHINGS!", 15, slug => GetAsyncList(slug, "no-match"));

        Assert.Equal("this-is-a", slug);
    }

    [Fact]
    public async Task AsyncUniqueTitleSlugWithTheLotWithMatch()
    {
        var expected = "this-is-a";
        var slug = await Slug.ToUniqueTitleSlugAsync("This is a test @6!%4 _- tHINGS!", 15, slug => GetAsyncList(slug, expected));

        Assert.NotEqual(expected, slug);
        Assert.StartsWith(expected, slug);
        Assert.Equal(expected.Length + 6, slug.Length);
    }
    #endregion

    #region CodeSlug Tests
    [Fact]
    public void CodeSlugWithTheLot()
    {
        var slug = Slug.ToCodeSlug("This is a test @6!%4 _- tHINGS!");

        Assert.Equal("This-is-a-test-6-4-tHINGS", slug);
    }

    [Fact]
    public void CodeSlugWithTheLotAndLimit()
    {
        var slug = Slug.ToCodeSlug("This is a test @6!%4 _- tHINGS!", 5);

        Assert.Equal("This-", slug);
    }

    [Fact]
    public void UniqueCodeSlugWithTheLotNoMatch()
    {
        var slug = Slug.ToUniqueCodeSlug("This is a test @6!%4 _- tHINGS!", 15, new List<string> { "foo", "this-is-a" });

        Assert.Equal("This-is-a", slug);
    }

    [Fact]
    public void UniqueCodeSlugWithTheLotWithMatch()
    {
        var expected = "This-is-a";
        var slug = Slug.ToUniqueCodeSlug("This is a test @6!%4 _- tHINGS!", 15, new List<string> { "foo", expected });

        Assert.NotEqual(expected, slug);
        Assert.StartsWith(expected, slug);
        Assert.Equal(expected.Length + 6, slug.Length);
    }

    [Fact]
    public async Task AsyncUniqueCodeSlugWithTheLotNoMatch()
    {
        var slug = await Slug.ToUniqueCodeSlugAsync("This is a test @6!%4 _- tHINGS!", 15, slug => GetAsyncList(slug, "no-match"));

        Assert.Equal("This-is-a", slug);
    }

    [Fact]
    public async Task AsyncUniqueCodeSlugWithTheLotWithMatch()
    {
        var expected = "This-is-a";
        var slug = await Slug.ToUniqueCodeSlugAsync("This is a test @6!%4 _- tHINGS!", 15, slug => GetAsyncList(slug, expected));

        Assert.NotEqual(expected, slug);
        Assert.StartsWith(expected, slug);
        Assert.Equal(expected.Length + 6, slug.Length);
    }
    #endregion

    #region Edge Cases And Coverage Boosters
    [Fact]
    public void ToUniqueTitleSlugMaxLengthLessThan6()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Slug.ToUniqueTitleSlug("This is a test @6!%4 _- tHINGS!", 5, new List<string> { "foo", "This-is-a" }));
    }

    [Fact]
    public void ToUniqueTitleSlugMaxLengthBiggerThanName()
    {
        var slug = Slug.ToUniqueTitleSlug("This", 50, new List<string> { "foo", "This-is-a" });

        Assert.Equal("this", slug);
    }

    [Theory]
    [InlineData("Test The")]
    [InlineData("Test Ltd")]
    [InlineData("Test Pty")]
    [InlineData("Test S.A")]
    [InlineData("Test Inc")]
    [InlineData("Test Incorporated")]
    [InlineData("Test Pty td")]
    [InlineData("Test (Inc)")]
    public void TrimAllTheEndings(string name)
    {
        var slug = Slug.ToSlug(name, false);

        Assert.Equal("Test", slug);
    }

    [Fact]
    public void RandomWebStringIsRandomEnough()
    {
        var seenSlugs = new List<string>();
        for(int i = 0; i < 20; i++) {
            var slug = Slug.RandomWebString(5);
            Assert.DoesNotContain(slug, seenSlugs);
            seenSlugs.Add(slug);
        }
    }
    #endregion

    private static async Task<bool> GetAsyncList(string slug, string expected) => await Task.FromResult(slug == expected);
}

