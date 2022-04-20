using ExtraDry.Server.Internal;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ExtraDry.Core.Tests.Server.Internal {

    public class ModelDescrptionTests {

        [Fact]
        public void KeyPropertyIsKeyAttribute()
        {
            var modelDescription = new ModelDescription(typeof(KeyAttributeEntity));

            Assert.NotNull(modelDescription);
            Assert.NotNull(modelDescription.StabilizerProperty);
            Assert.Equal("PrimaryKey", modelDescription.StabilizerProperty.ExternalName);
        }

        [Fact]
        public void KeyPropertyIsIdConvention()
        {
            var modelDescription = new ModelDescription(typeof(IdConventionEntity));

            Assert.NotNull(modelDescription);
            Assert.NotNull(modelDescription.StabilizerProperty);
            Assert.Equal("Id", modelDescription.StabilizerProperty.ExternalName);
        }

        [Fact]
        public void KeyPropertyIsClassNameConvention()
        {
            var modelDescription = new ModelDescription(typeof(ClassNameConventionEntity));

            Assert.NotNull(modelDescription);
            Assert.NotNull(modelDescription.StabilizerProperty);
            Assert.Equal("ClassNameConventionEntityId", modelDescription.StabilizerProperty.ExternalName);
        }

        [Fact]
        public void KeyPropertyIsMissing()
        {
            var exception = Assert.Throws<DryException>(() => new ModelDescription(typeof(NoImplicitStabilizer)));

            Assert.NotNull(exception);
            Assert.Equal("Unable to Sort. 0x0F3F241C", exception.UserMessage);
        }

        public class KeyAttributeEntity {

            [Key]
            public int PrimaryKey { get; set; }

            public string Payload { get; set; } = string.Empty;

        }

        public class IdConventionEntity {
            public int Id { get; set; }

            public string Payload { get; set; } = string.Empty;
        }

        public class ClassNameConventionEntity {
            public int ClassNameConventionEntityId { get; set; }

            public string Payload { get; set; } = string.Empty;
        }

        public class NoImplicitStabilizer {
            public int PrimaryKey { get; set; }

            public string Payload { get; set; } = string.Empty;
        }
    }
}
