using ExtraDry.Core;
using Sample.Shared;

namespace Sample.Tests.Shared.Entities;

public class RegionTests {

    [Theory]
    [InlineData("Id", 2)]
    [InlineData("Slug", "US")]
    [InlineData("Level", RegionLevel.Subdivision)]
    [InlineData("Title", "USA")]
    [InlineData("Description", "United States of America")]

    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var region = ValidRegion;
        var property = region.GetType().GetProperty(propertyName)!;

        property.SetValue(region, propertyValue);
        var result = property.GetValue(region);

        Assert.Equal(propertyValue, result);
    }

    [Fact]
    public void ValidateValidRegion()
    {
        var request = ValidRegion;
        var validator = new DataValidator();

        validator.ValidateObject(request);

        Assert.Empty(validator.Errors);
    }

    [Theory]
    [InlineData("Title", "")] // required
    [InlineData("Title", "0123456789012345678901234567890123456789")]
    [InlineData("Description", "")]
    [InlineData("Description", "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123")]
    public void InvalidPropertyValue(string propertyName, string propertyValue)
    {
        var request = ValidRegion;
        var validator = new DataValidator();
        var property = request.GetType().GetProperty(propertyName)!;

        property.SetValue(request, propertyValue);
        validator.ValidateObject(request);

        Assert.NotEmpty(validator.Errors);
    }

    [Theory]
    [InlineData(RegionLevel.Country, "AU")]
    [InlineData(RegionLevel.Country, "US")]
    [InlineData(RegionLevel.Subdivision, "AU-QLD")]
    [InlineData(RegionLevel.Subdivision, "US-CO")]
    public void ValidCodesForLevel(RegionLevel level, string code)
    {
        var request = ValidRegion;
        request.Slug = code;
        request.Level = level;
        var validator = new DataValidator();

        validator.ValidateObject(request);

        Assert.Empty(validator.Errors);
    }

    [Theory]
    [InlineData(RegionLevel.Country, "AUS")]
    [InlineData(RegionLevel.Country, "U")]
    [InlineData(RegionLevel.Country, "")]
    [InlineData(RegionLevel.Country, "W!")]
    [InlineData(RegionLevel.Subdivision, "AU-Q")]
    [InlineData(RegionLevel.Subdivision, "AU-QABCDEF")]
    [InlineData(RegionLevel.Locality, "AU-QLD-")]
    [InlineData(RegionLevel.Subdivision, "AU-QLD-ThisSuburbHasTooLongOfAName")]
    // valid codes at wrong level
    [InlineData(RegionLevel.Country, "AU-QLD")]
    [InlineData(RegionLevel.Country, "AU-QLD-Brisbane")]
    [InlineData(RegionLevel.Subdivision, "AU")]
    [InlineData(RegionLevel.Subdivision, "AU-QLD-Brisbane")]
    [InlineData(RegionLevel.Locality, "AU")]
    [InlineData(RegionLevel.Locality, "AU-QLD")]
    public void InvalidCodesForLevel(RegionLevel level, string code)
    {
        var request = ValidRegion;
        request.Slug = code;
        request.Level = level;
        var validator = new DataValidator();

        validator.ValidateObject(request);

        Assert.NotEmpty(validator.Errors);
    }

    [Theory]
    [InlineData("Id", 123456)]
    [InlineData("Level", (int)RegionLevel.Locality)] // don't show number through Strata
    public void JsonIgnoreValue(string propertyName, object propertyValue)
    {
        var request = ValidRegion;
        var property = request.GetType().GetProperty(propertyName)!;

        property.SetValue(request, propertyValue);
        var json = JsonSerializer.Serialize(request);

        // Check for the value as a complete json value, else the int or string could appear in the Uuid
        Assert.DoesNotContain($":{propertyValue}", json);
        Assert.DoesNotContain($":\"{propertyValue}\"", json);
    }

    public static Region ValidRegion => new() {
        Id = 1,
        Slug = "AU",
        Level = RegionLevel.Country,
        Title = "Australia",
        Description = "Commonwealth of Australia",
    };

}
