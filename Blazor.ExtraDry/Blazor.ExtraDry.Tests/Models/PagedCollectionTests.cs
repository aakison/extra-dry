using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Blazor.ExtraDry.Tests.Models {
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
            Assert.Null(target.Stabalizer);
            Assert.Equal(0, target.Total);
            Assert.Null(target.ContinuationToken);
        }

        [Theory]
        [InlineData("Filter", "Any")]
        [InlineData("Sort", "Any")]
        [InlineData("Stabalizer", "Any")]
        [InlineData("Total", 123)]
        [InlineData("ContinuationToken", "Any")] // token is retained, but not validated.
        public void RoundtripProperties(string propertyName, object propertyValue)
        {
            var target = new PagedCollection<object>();
            var property = target.GetType().GetProperty(propertyName);

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
                Stabalizer = "stabalizer",
                Start = 0,
                Total = 1,
            };
            var item = new Payload { Pay = "pay", Load = "load" };
            target.Items.Add(item);

            var json = JsonSerializer.Serialize(target);
            var result = JsonSerializer.Deserialize<PagedCollection<Payload>>(json);

            Assert.NotSame(result, target);
            Assert.NotSame(result.Items.First(), target.Items.First());
            Assert.Equal("pay", result.Items.First().Pay);
            Assert.Equal("load", result.Items.First().Load);
            Assert.Equal("filter", result.Filter);
            Assert.Equal("sort", result.Sort);
            Assert.Equal("stabalizer", result.Stabalizer);
            Assert.Equal(0, result.Start);
            Assert.Equal(1, result.Total);
        }

        private class Payload {
            public string Pay { get; set; }

            public string Load { get; set; }
        }

    }
}
