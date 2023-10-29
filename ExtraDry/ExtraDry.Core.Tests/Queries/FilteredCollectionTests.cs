using System.Text.Json;

namespace ExtraDry.Server.Tests.Models;

public class FilteredCollectionTests {

    [Fact]
    public void DefaultConstructor()
    {
        var collection = new FilteredCollection<object>();

        Assert.Empty(collection.Items);
        Assert.Equal(0, collection.Count);
        Assert.True(DateTime.UtcNow >= collection.Created);
        Assert.True(DateTime.UtcNow.AddSeconds(-1) < collection.Created);
        Assert.Null(collection.Filter);
    }

    [Theory]
    [InlineData(nameof(FilteredCollection<object>.Filter), "Any")]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var target = new FilteredCollection<object>();
        var property = target.GetType().GetProperty(propertyName) 
            ?? throw new ArgumentException("Bad argument", nameof(propertyValue));

        property.SetValue(target, propertyValue);
        var result = property.GetValue(target);

        Assert.Equal(propertyValue, result);
    }

    [Fact]
    public void JsonSerializable()
    {
        var target = new FilteredCollection<Payload>() {
            Filter = "filter",
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var json = JsonSerializer.Serialize(target);
        var result = JsonSerializer.Deserialize<FilteredCollection<Payload>>(json) ?? throw new Exception();

        Assert.NotSame(result, target);
        Assert.NotSame(result.Items.First(), target.Items.First());
        Assert.Equal("pay", result.Items.First().Pay);
        Assert.Equal("load", result.Items.First().Load);
        Assert.Equal("filter", result.Filter);
    }

    [Fact]
    public void CollectionCasting()
    {
        var target = new FilteredCollection<Payload>() {
            Filter = "filter",
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var iPayloadItems = target.Cast<IPayload>();

        Assert.Equal(1, iPayloadItems.Count);
        Assert.Equal(target.Items.First(), iPayloadItems.Items.First());
        Assert.Equal(target.Filter, iPayloadItems.Filter);
    }

    private interface IPayload {
        string Pay { get; set; }
    }

    private class Payload : IPayload {
        public string Pay { get; set; } = string.Empty;

        public string Load { get; set; } = string.Empty;
    }

}
