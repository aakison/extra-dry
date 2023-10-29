using System.Text.Json;

namespace ExtraDry.Server.Tests.Models;

public class BaseCollectionTests {

    [Fact]
    public void DefaultConstructor()
    {
        var collection = new BaseCollection<object>();

        Assert.Empty(collection.Items);
        Assert.Equal(0, collection.Count);
        Assert.True(DateTime.UtcNow >= collection.Created);
        Assert.True(DateTime.UtcNow.AddSeconds(-1) < collection.Created);
    }

    [Fact]
    public void JsonSerializable()
    {
        var target = new BaseCollection<Payload>();
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var json = JsonSerializer.Serialize(target);
        var result = JsonSerializer.Deserialize<BaseCollection<Payload>>(json) ?? throw new Exception();

        Assert.NotSame(result, target);
        Assert.NotSame(result.Items.First(), target.Items.First());
        Assert.Equal("pay", result.Items.First().Pay);
        Assert.Equal("load", result.Items.First().Load);
    }

    [Fact]
    public void CollectionCasting()
    {
        var target = new BaseCollection<Payload>();
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var iPayloadItems = target.Cast<IPayload>();

        Assert.Equal(1, iPayloadItems.Count);
        Assert.Equal(target.Items.First(), iPayloadItems.Items.First());
    }

    private interface IPayload {
        string Pay { get; set; }
    }

    private class Payload : IPayload {
        public string Pay { get; set; } = string.Empty;

        public string Load { get; set; } = string.Empty;
    }

}
