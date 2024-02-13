namespace ExtraDry.Core.Tests.Helpers;

public class DataValidatorExpandoTests
{

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
    [InlineData("ABN", "012345678")]
    [InlineData("IsActive", true)]
    [InlineData("IsActive", false)]
    [InlineData("Employees", 10)]
    [InlineData("Employees", 10.5)]
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
    [InlineData("ABN", "")] // Required
    [InlineData("ABN", null)] // Required
    [InlineData("ASX_CODE", 12)] // Int when expect string
    [InlineData("ASX_CODE", true)] // bool when expect string
    [InlineData("IsActive", 1)] // Number when expect bool
    [InlineData("IsActive", 10.5)] // Number when expect bool
    [InlineData("IsActive", "Yes")] // string when expect bool
    [InlineData("IsActive", "false")] // string bool when expect literal bool
    [InlineData("Employees", "Yes")] // String when expect Number
    [InlineData("Employees", "10")] // String Number when expect Json Number
    [InlineData("Employees", false)] // Bool when expect Number
    public void InvalidValue(string key, object? value)
    {
        var validator = new DataValidator();
        var sample = SchemaSample;
        sample.Values[key] = value;

        var success = validator.ValidateObject(sample);

        Assert.False(success);
        Assert.Single(validator.Errors);
    }

    public class Sample
    {

        public ExpandoValues Values { get; set; } = [];

    }

    public static ExpandoSchema Schema => new() {
        TargetType = typeof(Sample).Name,
        Fields = [
            new() { DataType = ExpandoDataType.Text, Slug = "ABN", MaxLength = 10, IsRequired = true },
            new() { DataType = ExpandoDataType.Text, Slug = "ASX_CODE", MaxLength = 10, IsRequired = false },
            new() { DataType = ExpandoDataType.Boolean, Slug = "IsActive" },
            new() { DataType = ExpandoDataType.Number, Slug = "Employees" },
            new() { DataType = ExpandoDataType.DateTime, Slug = "Incorporated" },
        ]
    };

    public static Sample SchemaSample {
        get {
            var sample = new Sample();
            sample.Values.Schema = Schema;
            sample.Values.Add("ABN", "0123456789"); // required, so default it.
            return sample;
        }
    }

}
