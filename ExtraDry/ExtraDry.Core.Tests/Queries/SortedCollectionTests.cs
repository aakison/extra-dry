using System.Text.Json;

namespace ExtraDry.Server.Tests.Models;

public class SortedCollectionTests
{

    [Fact]
    public void DefaultConstructor()
    {
        var collection = new SortedCollection<object>();

        Assert.Empty(collection.Items);
        Assert.Equal(0, collection.Count);
        Assert.True(DateTime.UtcNow >= collection.Created);
        Assert.True(DateTime.UtcNow.AddSeconds(-1) < collection.Created);
        Assert.Null(collection.Filter);
        Assert.Null(collection.Sort);
    }

    [Theory]
    [InlineData(nameof(SortedCollection<object>.Filter), "Any")]
    [InlineData(nameof(SortedCollection<object>.Sort), "Any")]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var target = new SortedCollection<object>();
        var property = target.GetType().GetProperty(propertyName)
            ?? throw new ArgumentException("Bad argument", nameof(propertyValue));

        property.SetValue(target, propertyValue);
        var result = property.GetValue(target);

        Assert.Equal(propertyValue, result);
    }

    [Fact]
    public void JsonSerializable()
    {
        var target = new SortedCollection<Payload>() {
            Filter = "filter",
            Sort = "sort",
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var json = JsonSerializer.Serialize(target);
        var result = JsonSerializer.Deserialize<SortedCollection<Payload>>(json) ?? throw new Exception();

        Assert.NotSame(result, target);
        Assert.NotSame(result.Items.First(), target.Items.First());
        Assert.Equal("pay", result.Items.First().Pay);
        Assert.Equal("load", result.Items.First().Load);
        Assert.Equal("filter", result.Filter);
        Assert.Equal("sort", result.Sort);
    }

    [Fact]
    public void CollectionCasting()
    {
        var target = new SortedCollection<Payload>() {
            Filter = "filter",
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var iPayloadItems = target.Cast<IPayload>();

        Assert.Equal(1, iPayloadItems.Count);
        Assert.Equal(target.Items.First(), iPayloadItems.Items.First());
        Assert.Equal(target.Filter, iPayloadItems.Filter);
        Assert.Equal(target.Sort, iPayloadItems.Sort);
    }

    private interface IPayload
    {
        string Pay { get; set; }
    }

    private sealed class Payload : IPayload
    {
        public string Pay { get; set; } = string.Empty;

        public string Load { get; set; } = string.Empty;
    }

}
