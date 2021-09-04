#nullable enable

using ExtraDry.Core;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace ExtraDry.Core.Tests.Helpers {
    public class DataValidatorTests {

        [Fact]
        public void DefaultValues()
        {
            var validator = new DataValidator();

            Assert.Empty(validator.Errors);
        }

        [Fact]
        public void FailValidationOfObject()
        {
            var sample = InvalidSample;
            var validator = new DataValidator();

            validator.ValidateObject(sample);

            Assert.NotEmpty(validator.Errors);
        }

        [Fact]
        public void ValidationOfObject()
        {
            var sample = ValidSample;
            var validator = new DataValidator();

            validator.ValidateObject(sample);

            Assert.Empty(validator.Errors);
        }

        [Fact]
        public void ValidProperty()
        {
            var sample = PartiallyValidSample;
            var validator = new DataValidator();

            validator.ValidateProperties(sample, nameof(Sample.Text));

            Assert.Empty(validator.Errors);
        }

        [Fact]
        public void InvalidProperty()
        {
            var sample = PartiallyValidSample;
            var validator = new DataValidator();

            validator.ValidateProperties(sample, nameof(Sample.Second));

            Assert.NotEmpty(validator.Errors);
        }

        [Fact]
        public void ExceptionCheckingInvalid()
        {
            var sample = InvalidSample;
            var validator = new DataValidator();

            validator.ValidateObject(sample);
            Assert.Throws<ValidationException>(() => validator.ThrowIfInvalid());
        }

        [Fact]
        public void ExceptionCheckingValid()
        {
            var sample = ValidSample;
            var validator = new DataValidator();

            validator.ValidateObject(sample);
            validator.ThrowIfInvalid(); // should be no-op
        }

        [Fact]
        public void MissingPropertyIsInvalid()
        {
            var sample = ValidSample;
            var validator = new DataValidator();

            validator.ValidateProperties(sample, "NoProperty");

            Assert.NotEmpty(validator.Errors);
        }

        [Fact]
        public void MissingPropertyForceCheck()
        {
            var sample = ValidSample;
            var validator = new DataValidator();

            validator.ValidateProperties(sample, true, "Ignored");

            Assert.NotEmpty(validator.Errors);
        }

        [Fact]
        public void MissingPropertyNoForceCheck()
        {
            var sample = ValidSample;
            var validator = new DataValidator();

            validator.ValidateProperties(sample, false, "Ignored");

            Assert.Empty(validator.Errors);
        }

        public class Sample {

            [Required]
            public string? Text { get; set; }

            [Required]
            public string? Second { get; set; }

            public string? Ignored { get; set; }

        }

        private static Sample ValidSample => new() {
            Text = "something",
            Second = "something else",
        };

        private static Sample InvalidSample => new() {
            Text = null,
            Second = null
        };

        private static Sample PartiallyValidSample => new() {
            Text = "something",
            Second = null,
        };

    }
}
