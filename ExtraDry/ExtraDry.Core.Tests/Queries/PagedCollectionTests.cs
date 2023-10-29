using System.Text.Json;

namespace ExtraDry.Server.Tests.Models;

public class PagedCollectionTests {

    [Fact]
    public void DefaultConstructor()
    {
        var target = new PagedCollection<object>();

        Assert.Empty(target.Items);
        Assert.Equal(0, target.Count);
        Assert.True(DateTime.UtcNow >= target.Created);
        Assert.True(DateTime.UtcNow.AddSeconds(-1) < target.Created);
        Assert.Null(target.Filter);
        Assert.Null(target.Sort);
        Assert.Equal(0, target.Total);
        Assert.Null(target.ContinuationToken);
    }

    [Theory]
    [InlineData("Filter", "Any")]
    [InlineData("Sort", "Any")]
    [InlineData("Total", 123)]
    [InlineData("ContinuationToken", "Any")] // token is retained, but not validated.
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var target = new PagedCollection<object>();
        var property = target.GetType().GetProperty(propertyName) ?? throw new ArgumentException("Can't find property", nameof(propertyValue));

        property.SetValue(target, propertyValue);
        var result = property.GetValue(target);

        Assert.Equal(propertyValue, result);
    }

    [Fact]
    public void JsonSerializable()
    {
        var target = new PagedCollection<Payload>() {
            Filter = "filter",
            Sort = "sort",
            Start = 0,
            Total = 1,
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var json = JsonSerializer.Serialize(target);
        var result = JsonSerializer.Deserialize<PagedCollection<Payload>>(json) ?? throw new Exception();

        Assert.NotSame(result, target);
        Assert.NotSame(result.Items.First(), target.Items.First());
        Assert.Equal("pay", result.Items.First().Pay);
        Assert.Equal("load", result.Items.First().Load);
        Assert.Equal("filter", result.Filter);
        Assert.Equal("sort", result.Sort);
        Assert.Equal(0, result.Start);
        Assert.Equal(1, result.Total);
    }

    [Fact]
    public void CollectionCasting()
    {
        var target = new PagedCollection<Payload>() {
            Filter = "filter",
            Sort = "sort",
            Start = 50,
            Total = 100,
            ContinuationToken = "TESTTOKEN",
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var iPayloadItems = target.Cast<IPayload>();

        Assert.Equal(1, iPayloadItems.Count);
        Assert.Equal(target.Items.First(), iPayloadItems.Items.First());
        Assert.Equal(target.Filter, iPayloadItems.Filter);
        Assert.Equal(target.Sort, iPayloadItems.Sort);
        Assert.Equal(target.Start, iPayloadItems.Start);
        Assert.Equal(target.Total, iPayloadItems.Total);
        Assert.Equal(target.ContinuationToken, iPayloadItems.ContinuationToken);
    }

    private interface IPayload {
        string Pay { get; set; }
    }

    private class Payload : IPayload {
        public string Pay { get; set; } = string.Empty;

        public string Load { get; set; } = string.Empty;
    }

}
