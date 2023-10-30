namespace ExtraDry.Server.Tests.Models;

public class StatisticsQueryableTests {

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UnfilteredStatistics(string value)
    {
        var filter = new FilterQuery { Filter = value };
        var models = Samples.Models;
        var expected = models.ToList();
        var greekCount = models.Count(e => e.Type == ModelType.Greek);
        var phoneticCount = models.Count(e => e.Type == ModelType.Phonetic);
        var hendrixCount = models.Count(e => e.Type == ModelType.Hendrix);

        var actual = models.AsQueryable().QueryWith(filter).ToStatistics();

        var byModelType = actual.Distributions!.First(e => e.PropertyName == nameof(Model.Type)).Counts;
        Assert.Equal(greekCount, byModelType[nameof(ModelType.Greek)]);
        Assert.Equal(phoneticCount, byModelType[nameof(ModelType.Phonetic)]);
        Assert.Equal(hendrixCount, byModelType[nameof(ModelType.Hendrix)]);
        Assert.Null(actual.Filter);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task UnfilteredStatisticsAsync(string value)
    {
        var filter = new FilterQuery { Filter = value };
        var models = Samples.Models;
        var greekCount = models.Count(e => e.Type == ModelType.Greek);
        var phoneticCount = models.Count(e => e.Type == ModelType.Phonetic);
        var hendrixCount = models.Count(e => e.Type == ModelType.Hendrix);

        var actual = await models.AsQueryable().QueryWith(filter).ToStatisticsAsync();

        var byModelType = actual.Distributions!.First(e => e.PropertyName == nameof(Model.Type)).Counts;
        Assert.Equal(greekCount, byModelType[nameof(ModelType.Greek)]);
        Assert.Equal(phoneticCount, byModelType[nameof(ModelType.Phonetic)]);
        Assert.Equal(hendrixCount, byModelType[nameof(ModelType.Hendrix)]);
        Assert.Null(actual.Filter);
    }

    [Theory]
    [InlineData("Alpha Beta")]
    [InlineData(" Alpha Beta ")]
    public async Task FilteredEnumStatisticsAsync(string value)
    {
        var filter = new FilterQuery { Filter = value };
        var models = Samples.Models;
        var subset = models.Where(e => e.Name.Contains("Alpha") || e.Name.Contains("Beta"));
        var greekCount = subset.Count(e => e.Type == ModelType.Greek);
        var phoneticCount = subset.Count(e => e.Type == ModelType.Phonetic);
        var hendrixCount = subset.Count(e => e.Type == ModelType.Hendrix);

        var actual = await models.AsQueryable().QueryWith(filter).ToStatisticsAsync();

        var byModelType = actual.Distributions!.First(e => e.PropertyName == nameof(Model.Type)).Counts;
        Assert.Equal(greekCount, byModelType[nameof(ModelType.Greek)]);
        Assert.Equal(phoneticCount, byModelType[nameof(ModelType.Phonetic)]);
        Assert.False(byModelType.ContainsKey(nameof(ModelType.Hendrix)));
        Assert.Equal("Alpha Beta", actual.Filter);
    }

    [Fact]
    public async Task FilteredStringStatisticsAsync()
    {
        var filter = new FilterQuery { Filter = "Alpha Beta" };
        var models = Samples.Models;
        var alphaCount = models.Count(e => e.Name == "Alpha");
        var betaCount = models.Count(e => e.Name == "Beta");

        var actual = await models.AsQueryable().QueryWith(filter).ToStatisticsAsync();

        var byName = actual.Distributions!.First(e => e.PropertyName == nameof(Model.Name)).Counts;
        Assert.Equal(alphaCount, byName["Alpha"]);
        Assert.Equal(betaCount, byName["Beta"]);
        Assert.False(byName.ContainsKey("Charlie"));
    }

}
