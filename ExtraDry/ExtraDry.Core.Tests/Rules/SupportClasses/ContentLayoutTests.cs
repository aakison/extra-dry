#nullable enable

using ExtraDry.Core;
using System;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace ExtraDry.Core.Tests.Rules.SupportClasses {

    public class ContentLayoutTests {

        [Fact]
        public void ValidateContent()
        {
            var content = ValidContent;
            var validator = new DataValidator();

            validator.ValidateObject(content);

            Assert.Empty(validator.Errors);
        }

        [Theory]
        [InlineData("Sections", null)]
        public void RoundtripLayoutProperties(string propertyName, object propertyValue)
        {
            var content = ValidContent;
            var property = content.GetType().GetProperty(propertyName);

            property?.SetValue(content, propertyValue);
            var result = property?.GetValue(content);

            Assert.Equal(propertyValue, result);
        }

        [Theory]
        [InlineData("Layout", SectionLayout.Single)]
        [InlineData("Theme", ContentTheme.Dark)]
        [InlineData("Containers", null)]
        public void RoundtripSectionProperties(string propertyName, object propertyValue)
        {
            var content = ValidContent;
            var section = content.Sections.First();
            var property = section.GetType().GetProperty(propertyName);

            property?.SetValue(section, propertyValue);
            var result = property?.GetValue(section);

            Assert.Equal(propertyValue, result);
        }

        [Fact]
        public void SectionEnumAsString()
        {
            var content = ValidContent;
            var section = content.Sections.First();

            var json = JsonSerializer.Serialize(section);

            Assert.Contains(section.Theme.ToString(), json);
            Assert.Contains(section.Layout.ToString(), json);
        }

        [Theory]
        [InlineData("Html", "X")]
        [InlineData("Padding", ContentPadding.Double)]
        [InlineData("Alignment", ContentAlignment.MiddleCenter)]
        public void RoundtripContainerProperties(string propertyName, object propertyValue)
        {
            var content = ValidContent;
            var container = content.Sections.First().Containers.First();
            var property = container.GetType().GetProperty(propertyName);

            property?.SetValue(container, propertyValue);
            var result = property?.GetValue(container);

            Assert.Equal(propertyValue, result);
        }

        [Fact]
        public void RoundtripContainerGuid()
        {
            var content = ValidContent;
            var container = content.Sections.First().Containers.First();
            var guid = Guid.NewGuid();

            container.Id = guid;

            Assert.Equal(guid, container.Id);
        }

        [Fact]
        public void ContainerEnumAsString()
        {
            var content = ValidContent;
            var container = content.Sections.First().Containers.First();

            var json = JsonSerializer.Serialize(container);

            Assert.Contains(container.Alignment.ToString(), json);
            Assert.Contains(container.Padding.ToString(), json);
        }

        [Theory]
        [InlineData(SectionLayout.Double, 2)]
        [InlineData(SectionLayout.DoubleWeightedLeft, 2)]
        [InlineData(SectionLayout.DoubleWeightedRight, 2)]
        [InlineData(SectionLayout.Quadruple, 4)]
        [InlineData(SectionLayout.Single, 1)]
        [InlineData(SectionLayout.Triple, 3)]
        public void SectionColumnCountsMatchLayout(SectionLayout layout, int count)
        {
            var content = ValidContent;
            var section = content.Sections.First();

            section.Layout = layout;
            var toDisplay = section.DisplayContainers;

            Assert.Equal(count, toDisplay.Count());
        }

        //[Fact]
        //public void RoundtripUniqueId()
        //{
        //    var blob = ValidBlob;
        //    var guid = Guid.NewGuid();

        //    blob.UniqueId = guid;

        //    Assert.Equal(guid, blob.UniqueId);
        //}

        //[Fact]
        //public void IdDoesNotLeakToJson()
        //{
        //    var blob = ValidBlob;
        //    blob.Id = 12345;

        //    var json = JsonSerializer.Serialize(blob);

        //    Assert.DoesNotContain("12345", json);
        //}

        private static ContentLayout ValidContent => new() {
            Sections = {
                new ContentSection {
                    Containers = {
                        new ContentContainer {

                        }
                    }
                }
            }    
        };

    }
}
