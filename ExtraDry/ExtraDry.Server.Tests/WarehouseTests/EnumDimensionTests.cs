using ExtraDry.Server.DataWarehouse.Builder;

namespace ExtraDry.Server.Tests.WarehouseTests;

/// <summary>
/// Set of relatively useless tests, but removes distracting false-negatives from code coverage.
/// </summary>
public class EnumDimensionTests {

    [Theory]
    [InlineData("Id", 7)]
    [InlineData(nameof(EnumDimension.Description), "value")]
    [InlineData(nameof(EnumDimension.Name), "value")]
    [InlineData(nameof(EnumDimension.GroupName), "value")]
    [InlineData(nameof(EnumDimension.ShortName), "value")]
    [InlineData(nameof(EnumDimension.Order), 1)]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var builder = new EnumDimension();
        var property = builder.GetType().GetProperty(propertyName);

        property?.SetValue(builder, propertyValue);
        var result = property?.GetValue(builder);

        Assert.Equal(propertyValue, result);
    }

}
