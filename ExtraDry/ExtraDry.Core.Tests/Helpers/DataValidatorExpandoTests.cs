using System.Runtime.CompilerServices;

namespace ExtraDry.Core.Tests.Helpers;

public class DataValidatorExpandoTests {

    [Fact]
    public void EmptyExpandoValidates()
    {
        var validator = new DataValidator();
        var sample = SchemaSample;

        var success = validator.ValidateObject(sample);

        Assert.True(success);
        Assert.Empty(validator.Errors);
    }

    [Theory]
    [InlineData("ASX_CODE", "OK")]
    //[InlineData("ABN", "012345678")]
    public void ValidValue(string key, object value)
    {
        var validator = new DataValidator();
        var sample = SchemaSample;
        sample.Values[key] = value;

        var success = validator.ValidateObject(sample);

        Assert.True(success);
        Assert.Empty(validator.Errors);
    }

    [Theory]
    [InlineData("ASX_CODE", "OK0123456789")] // MaxLength
    //[InlineData("ABN", "")] // Required
    public void InalidValue(string key, object value)
    {
        var validator = new DataValidator();
        var sample = SchemaSample;
        sample.Values[key] = value;

        var success = validator.ValidateObject(sample);

        Assert.False(success);
        Assert.Equal(1, validator.Errors.Count);
    }

    public class Sample {

        public ExpandoValues Values { get; set; } = new();

    }

    public ExpandoSchema Schema => new ExpandoSchema {
        TargetType = typeof(Sample).Name,
        Fields = new List<ExpandoField> {
            //TODO: this requires more design, how to make required field when field input is optional???
            //new() { DataType = ExpandoDataType.Text, Slug = "ABN", MaxLength = 10, IsRequired = true },
            new() { DataType = ExpandoDataType.Text, Slug = "ASX_CODE", MaxLength = 10, IsRequired = false }
        }
    };

    public Sample SchemaSample {
        get {
            var sample = new Sample();
            sample.Values.Schema = Schema;
            return sample;
        }   
    }

}
