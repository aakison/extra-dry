﻿using System.Text.Json;

namespace ExtraDry.Core.Tests.Models;

public class HierarchyCollectionTests
{
    [Fact]
    public void DefaultConstructor()
    {
        var collection = new HierarchyCollection<object>();

        Assert.Empty(collection.Items);
        Assert.Equal(0, collection.Count);
        Assert.True(DateTime.UtcNow >= collection.Created);
        Assert.True(DateTime.UtcNow.AddSeconds(-1) < collection.Created);
        Assert.Null(collection.Filter);
        Assert.Null(collection.Sort);
        Assert.Null(collection.Level);
        Assert.Null(collection.Expand);
        Assert.Null(collection.Collapse);
    }

    [Theory]
    [InlineData(nameof(HierarchyCollection<object>.Filter), "Any")]
    [InlineData(nameof(HierarchyCollection<object>.Sort), "Any")]
    [InlineData(nameof(HierarchyCollection<object>.Level), 3)]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var target = new HierarchyCollection<object>();
        var property = target.GetType().GetProperty(propertyName)
            ?? throw new ArgumentException("Bad argument", nameof(propertyValue));

        property.SetValue(target, propertyValue);
        var result = property.GetValue(target);

        Assert.Equal(propertyValue, result);
    }

    [Theory]
    [InlineData(nameof(HierarchyCollection<object>.Expand))]
    [InlineData(nameof(HierarchyCollection<object>.Collapse))]
    public void RoundtripStringListProperties(string propertyName)
    {
        var target = new HierarchyCollection<object>();
        var property = target.GetType().GetProperty(propertyName)
            ?? throw new ArgumentException("Bad argument", nameof(propertyName));
        var array = new List<string> { "one", "two" };

        property.SetValue(target, array);
        var result = property.GetValue(target);

        Assert.Equal(array, result);
    }

    [Fact]
    public void JsonSerializable()
    {
        var target = new HierarchyCollection<Payload>() {
            Filter = "filter",
            Sort = "sort",
            Level = 3,
            Expand = ["one", "two"],
            Collapse = ["three", "four"],
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var json = JsonSerializer.Serialize(target);
        var result = JsonSerializer.Deserialize<HierarchyCollection<Payload>>(json) ?? throw new ArgumentException();

        Assert.NotSame(result, target);
        Assert.NotSame(result.Items.First(), target.Items.First());
        Assert.Equal("pay", result.Items.First().Pay);
        Assert.Equal("load", result.Items.First().Load);
        Assert.Equal("filter", result.Filter);
        Assert.Equal("sort", result.Sort);
        Assert.Equal(3, result.Level);
        Assert.Contains("one", result.Expand!);
        Assert.Contains("three", result.Collapse!);
    }

    [Fact]
    public void CollectionCasting()
    {
        var target = new HierarchyCollection<Payload>() {
            Filter = "filter",
            Sort = "sort",
            Level = 3,
            Expand = ["one", "two"],
            Collapse = ["three", "four"],
        };
        var item = new Payload { Pay = "pay", Load = "load" };
        target.Items.Add(item);

        var iPayloadItems = target.Cast<IPayload>();

        Assert.Equal(1, iPayloadItems.Count);
        Assert.Equal(target.Items.First(), iPayloadItems.Items.First());
        Assert.Equal(target.Filter, iPayloadItems.Filter);
        Assert.Equal(target.Sort, iPayloadItems.Sort);
        Assert.Equal(target.Level, iPayloadItems.Level);
        Assert.Contains("one", iPayloadItems.Expand!);
        Assert.Contains("three", iPayloadItems.Collapse!);
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
