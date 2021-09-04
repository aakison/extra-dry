using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Blazor.ExtraDry.Tests.Models {
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
            Assert.Null(collection.Sort);
            Assert.Null(collection.Stabilizer);
        }

        [Theory]
        [InlineData("Filter", "Any")]
        [InlineData("Sort", "Any")]
        [InlineData("Stabilizer", "Any")]
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var target = new FilteredCollection<object>();
            var property = target.GetType().GetProperty(propertyName);

            property.SetValue(target, propertyValue);
            var result = property.GetValue(target);

            Assert.Equal(propertyValue, result);
        }

        [Fact]
        public void JsonSerializable()
        {
            var target = new FilteredCollection<Payload>() {
                Filter = "filter",
                Sort = "sort",
                Stabilizer = "stabilizer",
            };
            var item = new Payload { Pay = "pay", Load = "load" };
            target.Items.Add(item);

            var json = JsonSerializer.Serialize(target);
            var result = JsonSerializer.Deserialize<FilteredCollection<Payload>>(json);

            Assert.NotSame(result, target);
            Assert.NotSame(result.Items.First(), target.Items.First());
            Assert.Equal("pay", result.Items.First().Pay);
            Assert.Equal("load", result.Items.First().Load);
            Assert.Equal("filter", result.Filter);
            Assert.Equal("sort", result.Sort);
            Assert.Equal("stabilizer", result.Stabilizer);
        }

        private class Payload {
            public string Pay { get; set; }

            public string Load { get; set; }
        }

    }
}
