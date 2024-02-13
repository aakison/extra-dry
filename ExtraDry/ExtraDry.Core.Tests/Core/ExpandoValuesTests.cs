using System.Text.Json;

namespace ExtraDry.Core.Tests;

public class ExpandoValuesTests
{
    private ExpandoSchema Schema { get; set; }

    public ExpandoValuesTests()
    {
        Schema = new ExpandoSchema {
            Fields = [
                new() { Slug = "external_id", IsRequired = true, MaxLength = 5, DataType = ExpandoDataType.Text, Label = "External ID", State = ExpandoState.Active },
                new() { Slug = "external_id_with_valid_values", IsRequired = true, MaxLength = 5, DataType = ExpandoDataType.Text, Label = "External ID", State = ExpandoState.Active, ValidValues = ["EX01","EX02", "EX03"] },
                new() { Slug = "building_construction_date", IsRequired = true, DataType = ExpandoDataType.DateTime, Label = "Building Constructed On", State = ExpandoState.Active },
                new() { Slug = "property_code", IsRequired = true, DataType = ExpandoDataType.Number, RangeMinimum = 10, RangeMaximum = 50, Label = "Property Code", State = ExpandoState.Active }
            ]
        };
    }

    [Fact]
    public void Serialize()
    {
        var obj = new ExpandoValues {
            { "f1", "val1" },
            { "f2", "val2" }
        };
        var json = JsonSerializer.Serialize(obj).Replace(" ", "");

        Assert.Equal(@"{""f1"":""val1"",""f2"":""val2""}", json);
    }

    [Theory]
    [InlineData("external_id", "EX01")]
    [InlineData("external_id_with_valid_values", "EX01")]
    [InlineData("building_construction_date", "1980-01-05")]
    [InlineData("property_code", 10)]
    public void ValidExpandoValues(string key, object value)
    {
        var values = new ExpandoValues {
                    { "external_id", "EX01" },
                    { "external_id_with_valid_values", "EX01" },
                    { "building_construction_date", "1980-01-05" },
                    { "property_code", 10 }
                };
        values[key] = value;
        var result = Schema.ValidateValues(values);
        Assert.Empty(result);
    }

    public static IEnumerable<object[]> InValidExpandoData =>
        new List<object[]> {
            new object[] {
                new ExpandoValues {
                    { "external_id", "EX0000000000001" },
                    { "external_id_with_valid_values", "EX05" },
                    { "building_construction_date", "InvalidDate" },
                    { "property_code", 51 }
                }
            },
            new object[] {
                new ExpandoValues {
                    { "external_id", 13298470 },
                    { "external_id_with_valid_values", "EXT01" },
                    { "building_construction_date", "" },
                    { "property_code", 100 }
                }
            },
        };
}
