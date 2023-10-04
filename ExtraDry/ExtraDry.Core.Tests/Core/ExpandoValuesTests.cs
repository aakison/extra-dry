namespace ExtraDry.Core.Tests.Core {

    public class ExpandoValuesTests {
        private ExpandoSchema Schema { get; set; }

        public ExpandoValuesTests()
        {
            Schema = new ExpandoSchema {
                Sections = new List<ExpandoSection>() {
                    {
                        new ExpandoSection {
                            Title = "Section 1",
                            State = ExpandoState.Active,
                            Order = 1,
                            Fields = new List<ExpandoField>() {
                                    new ExpandoField {  Slug =  "external_id", IsRequired = true, MaxLength = 5, DataType = ExpandoDataType.Text, Label = "External ID", State = ExpandoState.Active },
                                    new ExpandoField {  Slug =  "external_id_with_valid_values", IsRequired = true, MaxLength = 5, DataType = ExpandoDataType.Text, Label = "External ID", State = ExpandoState.Active, ValidValues = new List<string> { "EX01","EX02", "EX03" } },
                                    new ExpandoField {  Slug =  "building_construction_date", IsRequired = true, DataType = ExpandoDataType.Date, Label = "Building Constructed On", State = ExpandoState.Active },
                                    new ExpandoField {  Slug =  "property_code", IsRequired = true, DataType = ExpandoDataType.Number, RangeMinimum = 10, RangeMaximum = 50, Label = "Property Code", State = ExpandoState.Active }
                            }
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData(nameof(ValidExpandoData))]
        public void ValidExpandoValues(ExpandoValues expandoValue)
        {
            var result = Schema.ValidateValues(expandoValue);
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(InValidExpandoData))]
        public void InValidExpandoValues(ExpandoValues expandoValue)
        {
            var result = Schema.ValidateValues(expandoValue);

            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count());
        }

        public static IEnumerable<object[]> ValidExpandoData =>
            new List<object[]> {
                new object[] { new ExpandoValues { Values = new Dictionary<string, object>() { { "external_id", "EX01" }, { "external_id_with_valid_values", "EX03" }, { "building_construction_date", "23-05-1980" }, { "property_code", 10 } } }  },
                new object[] { new ExpandoValues { Values = new Dictionary<string, object>() { { "external_id", "10" }, { "external_id_with_valid_values", "EX02" }, { "building_construction_date", DateTime.Now.AddYears(-5) }, { "property_code", 15 } } } }
                };

        public static IEnumerable<object[]> InValidExpandoData =>
            new List<object[]> {
                new object[] { new ExpandoValues { Values = new Dictionary<string, object>() { { "external_id", "EX0000000000001" }, { "external_id_with_valid_values", "EX05" }, { "building_construction_date", "InvalidDate" }, { "property_code", 51 } } }  },
                new object[] { new ExpandoValues { Values = new Dictionary<string, object>() { { "external_id", 13298470 }, { "external_id_with_valid_values", "EXT01" },  { "building_construction_date", "" }, { "property_code", 100 } } } }
            };
    }
}
