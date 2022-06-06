using ExtraDry.Core;
using ExtraDry.Server.Internal;
using System.Linq;
using Xunit;

namespace ExtraDry.Core.Tests.Internals {

    public class FilterParserTests {

        [Theory]
        [InlineData("Name")]
        [InlineData("_Name")]
        [InlineData("Name123")]
        [InlineData("_Name123")]
        public void SingleRuleValue(string identifier)
        {
            var filter = $"{identifier}:Value";

            var tree = FilterParser.Parse(filter);

            Assert.Equal(identifier, tree.Rules.First().PropertyName);
            Assert.Equal("Value", tree.Rules.First().Values.First());
            Assert.Equal(BoundRule.None, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.None, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void SingleRuleDoubleValues()
        {
            var filter = "Name:Value1|Value2";

            var tree = FilterParser.Parse(filter);

            Assert.Equal("Name", tree.Rules.First().PropertyName);
            Assert.Equal(2, tree.Rules.First().Values.Count);
            Assert.Equal("Value1", tree.Rules.First().Values.First());
            Assert.Equal("Value2", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.None, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.None, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void SingleRuleQuotedValue()
        {
            var filter = @"Name:""Value One""";

            var tree = FilterParser.Parse(filter);

            Assert.Equal("Name", tree.Rules.First().PropertyName);
            Assert.Equal("Value One", tree.Rules.First().Values.First());
            Assert.Equal(BoundRule.None, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.None, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void SingleRuleCompositeValues()
        {
            var filter = @"Name:Value1|""Value Two""";

            var tree = FilterParser.Parse(filter);

            Assert.Equal("Name", tree.Rules.First().PropertyName);
            Assert.Equal(2, tree.Rules.First().Values.Count);
            Assert.Equal("Value1", tree.Rules.First().Values.First());
            Assert.Equal("Value Two", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.None, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.None, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void SingleRuleSeparateCompositeValues()
        {
            var filter = @"Name:Value1 Name:Value2";

            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("Name", tree.Rules.First().PropertyName);
            Assert.Equal(2, tree.Rules.First().Values.Count);
            Assert.Equal("Value1", tree.Rules.First().Values.First());
            Assert.Equal("Value2", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.None, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.None, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void DoubleRule()
        {
            var filter = @"Name1:Value1 Name2:Value2";

            var tree = FilterParser.Parse(filter);

            Assert.Equal(2, tree.Rules.Count);
            Assert.Equal("Name1", tree.Rules.First().PropertyName);
            Assert.Equal("Value1", tree.Rules.First().Values.First());
            Assert.Equal("Name2", tree.Rules.Last().PropertyName);
            Assert.Equal("Value2", tree.Rules.Last().Values.First());
            Assert.Equal(BoundRule.None, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.None, tree.Rules.First().UpperBound);
        }

        [Theory]
        [InlineData("1Name:Value1")]
        [InlineData("Name:~unquoted~")]
        [InlineData("Name:[]")]
        [InlineData("~~~totally invalid~~~")]
        //[InlineData("NameThatGoesNowhere")]
        //[InlineData("Name:  extra")]
        public void InvalidIdentifierRule(string filter)
        {
            Assert.Throws<DryException>(() => FilterParser.Parse(filter));
        }

        [Fact]
        public void InclusiveBetweenRangeRule()
        {
            var filter = @"Number:[10,20]";

            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("Number", tree.Rules.First().PropertyName);
            Assert.Equal("10", tree.Rules.First().Values.First());
            Assert.Equal("20", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.Inclusive, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.Inclusive, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void EsclusiveBetweenRangeRule()
        {
            var filter = @"Number:(10,20)";

            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("Number", tree.Rules.First().PropertyName);
            Assert.Equal("10", tree.Rules.First().Values.First());
            Assert.Equal("20", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.Exclusive, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.Exclusive, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void GreaterThanRule()
        {
            var filter = @"Number:[10,)";

            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("Number", tree.Rules.First().PropertyName);
            Assert.Equal("10", tree.Rules.First().Values.First());
            Assert.Equal("", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.Inclusive, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.Exclusive, tree.Rules.First().UpperBound);
        }

        [Fact]
        public void LessThanRule()
        {
            var filter = @"Number:[,20)";

            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("Number", tree.Rules.First().PropertyName);
            Assert.Equal("", tree.Rules.First().Values.First());
            Assert.Equal("20", tree.Rules.First().Values.Last());
            Assert.Equal(BoundRule.Inclusive, tree.Rules.First().LowerBound);
            Assert.Equal(BoundRule.Exclusive, tree.Rules.First().UpperBound);
        }

        [Theory]
        [InlineData("Adams", "Beck")] // Text
        [InlineData("12", "34")]  // Integer
        [InlineData("1.23", "2.34")] // Decimal
        [InlineData("1.23e2", "2.34e5")] // Exponential
        [InlineData("-12", "-34")]  // Negative Integer
        [InlineData("-1.23", "-2.34")] // Negative Decimal
        [InlineData("-1.23e2", "-2.34e5")] // Negative Exponential
        [InlineData("2021-01-01", "2021-02-01")] // Date
        [InlineData("2021-01-01T08:00", "2021-02-01T08:00")] // DateTime
        [InlineData(@"""Mc Adams""", @"""Mc Donald""")] // Quoted Space
        [InlineData("123", "")] // Greater than
        [InlineData("", "123")] // Less than
        public void ValidValuesInRange(string from, string to)
        {
            var filter = $"Number:[{from},{to})";

            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("Number", tree.Rules.First().PropertyName);
            Assert.Equal(from.Trim('"'), tree.Rules.First().Values.First());
            Assert.Equal(to.Trim('"'), tree.Rules.First().Values.Last());
        }

        [Theory]
        [InlineData("bob")]
        [InlineData("alice")]
        [InlineData(@"""asdf~ asdf""")]
        public void SingleValue(string filter)
        {
            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("*", tree.Rules.First().PropertyName);
            Assert.Equal(filter.Trim('"'), tree.Rules.First().Values.First());
        }

        [Theory]
        [InlineData("bob alice", "bob", "alice")]
        [InlineData(@"""asdf~ asdf"" alice", "asdf~ asdf", "alice")]
        [InlineData(@"asdf ""bob alice""", "asdf", "bob alice")]
        [InlineData(@"""ivan"" ""bob alice""", "ivan", "bob alice")]
        public void DoubleValue(string filter, string expectedFirst, string expectedSecond)
        {
            var tree = FilterParser.Parse(filter);

            Assert.Single(tree.Rules);
            Assert.Equal("*", tree.Rules.First().PropertyName);
            Assert.Equal(expectedFirst, tree.Rules.First().Values.First());
            Assert.Equal(expectedSecond, tree.Rules.First().Values.Last());
        }

        [Theory]
        [InlineData("bob number:123")]
        [InlineData(@"""bob"" number:123")]
        [InlineData(@"""bob"" number:""123""")]
        public void SingleValueThenNamedValue(string filter)
        {
            var tree = FilterParser.Parse(filter);

            Assert.Equal(2, tree.Rules.Count);
            Assert.Equal("*", tree.Rules.First().PropertyName);
            Assert.Equal("bob", tree.Rules.First().Values.First());
            Assert.Equal("number", tree.Rules.Last().PropertyName);
            Assert.Equal("123", tree.Rules.Last().Values.First());
        }

    }
}
