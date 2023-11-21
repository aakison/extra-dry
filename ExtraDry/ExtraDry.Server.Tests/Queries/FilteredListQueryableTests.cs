namespace ExtraDry.Server.Tests.Models;

public class FilteredListQueryableTests {

    [Fact]
    public void QueryableInterfacePublished()
    {
        var filter = new FilterQuery();
        var models = Samples.Models;

        var partialQueryable = models.AsQueryable().QueryWith(filter);

        Assert.NotNull(partialQueryable.ElementType);
        Assert.NotNull(partialQueryable.Expression);
        Assert.NotNull(partialQueryable.Provider);
        Assert.NotNull(partialQueryable.GetEnumerator());
        Assert.NotNull(((System.Collections.IEnumerable)partialQueryable).GetEnumerator());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task EmptyQueriesReturnEverything(string value)
    {
        var filter = new FilterQuery { Filter = value };
        var models = Samples.Models;
        var expected = models.ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task StringEqualsMatchNone()
    {
        var filter = new FilterQuery { Filter = "name:Bob" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name.Equals("Bob", StringComparison.OrdinalIgnoreCase)).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task StringEqualsMatchSingle()
    {
        var filter = new FilterQuery { Filter = "name:Bravo" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name.Equals("Bravo", StringComparison.OrdinalIgnoreCase)).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task StringEqualsMatchMultiple()
    {
        var filter = new FilterQuery { Filter = "name:Alpha" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name.Equals("Alpha", StringComparison.OrdinalIgnoreCase)).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task SingleIntEquals()
    {
        var filter = new FilterQuery { Filter = "Id:1" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Id == 1).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task MultipleIntEquals()
    {
        var filter = new FilterQuery { Filter = "Id:1|8" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Id == 1 || e.Id == 8).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task IntRangeReturnsNone()
    {
        var filter = new FilterQuery { Filter = "Id:[-2,-1)" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Id >= -2 && e.Id < -1).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task IntRangeReturnsMultiple()
    {
        var filter = new FilterQuery { Filter = "Id:[2,5)" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Id >= 2 && e.Id < 5).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task ValidEnumValueReturnsNone()
    {
        var filter = new FilterQuery { Filter = "type:latin" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Latin).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task InvalidFieldNameReturnsEmpty()
    {
        var filter = new FilterQuery { Filter = "invalid:latin" };
        var models = Samples.Models;

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Empty(actual.Items);
    }

    [Fact]
    public async Task InvalidEnumValueReturnsEmpty()
    {
        var filter = new FilterQuery { Filter = "type:invalid" };
        var models = Samples.Models;

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Empty(actual.Items);
    }

    [Fact]
    public async Task ValidEnumValueReturnsSingle()
    {
        var filter = new FilterQuery { Filter = "type:hendrix" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Hendrix).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task ValidEnumValueReturnsMultiple()
    {
        var filter = new FilterQuery { Filter = "type:greek" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Greek).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task UnnammedFilterReturnsNone()
    {
        var filter = new FilterQuery { Filter = "bob" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name == "bob" || e.Soundex.StartsWith("bob")).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Theory]
    [InlineData("Alpha")]
    [InlineData("A41")]
    public async Task UnnammedFilterMatchesSingleKeyword(string keyword)
    {
        var filter = new FilterQuery { Filter = keyword };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name.Equals(keyword, StringComparison.InvariantCultureIgnoreCase) || e.Soundex.StartsWith(keyword, StringComparison.InvariantCultureIgnoreCase)).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task UnnammedFilterMatchesIntValue()
    {
        var filter = new FilterQuery { Filter = "10" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Id == 10).ToList();

        var actual = await models.AsQueryable().QueryWith(filter).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task NonMatchingFilterWithDefaultLambdaReturnsNone()
    {
        var filter = new FilterQuery { Filter = "nothing" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name == "nothing" && e.Type == ModelType.Phonetic).ToList();

        var actual = await models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Phonetic).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task NoFilterWithDefaultLambdaReturnsSet(string value)
    {
        var filter = new FilterQuery { Filter = value };
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Phonetic).ToList();

        var actual = await models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Phonetic).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task MatchingFilterWithDefaultLambdaReturnsSingle()
    {
        var filter = new FilterQuery { Filter = "Alpha" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Name == "Alpha" && e.Type == ModelType.Phonetic).ToList();

        var actual = await models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Phonetic).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task MatchingFilterWithDefaultLambdaReturnsMultiple()
    {
        var filter = new FilterQuery { Filter = "id:[5,8]" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Id >= 5 && e.Id <= 8 && e.Type == ModelType.Phonetic).ToList();

        var actual = await models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Phonetic).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public async Task FilterOverridesLambdaReturnsMultiple()
    {
        var filter = new FilterQuery { Filter = "type:greek" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Greek).ToList();

        var actual = await models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Phonetic).ToFilteredCollectionAsync();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public void UnfilteredMethodReturnsAll()
    {
        var filter = new FilterQuery { Filter = "type:greek" };
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Greek).ToList();

        var actual = models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Phonetic).ToFilteredCollection();

        Assert.Equal(expected, actual.Items);
    }

    [Fact]
    public void DefaultFilterLambdaWithMultipleProperties()
    {
        var filter = new FilterQuery();
        var models = Samples.Models;
        var expected = models.Where(e => e.Type == ModelType.Greek && e.Notes != null).ToList();

        var actual = models.AsQueryable().QueryWith(filter, e => e.Type == ModelType.Greek && e.Notes != null).ToFilteredCollection();

        Assert.Equal(expected, actual.Items);
    }

}
