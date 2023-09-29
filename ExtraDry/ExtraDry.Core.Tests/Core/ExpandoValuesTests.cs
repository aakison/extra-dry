using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                                    new ExpandoField {  Slug =  "building_construction_date", IsRequired = true, RangeMinimum = "1970-01-01", RangeMaximum = "2023-01-01", DataType = ExpandoDataType.Date, Label = "Building Constructed On", State = ExpandoState.Active },
                                    new ExpandoField {  Slug =  "property_code", IsRequired = true, DataType = ExpandoDataType.Number, RangeMinimum = "10", RangeMaximum = "50", Label = "Property Code", State = ExpandoState.Active }
                            }
                        }
                    }
                }
            };

        }

        [Theory]
        [ClassData( typeof(ValidExpandoValuesData) )]
        public void ValidExpandoValues(List<ExpandoValues> expandoValues)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();

            foreach(var item in expandoValues) {
                result = Schema.ValidateValues(item);
            }

            Assert.Empty(result);
        }


        [Theory]
        [ClassData(typeof(InValidExpandoValuesData))]
        public void InValidExpandoValues(List<ExpandoValues> expandoValues)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();

            foreach(var item in expandoValues) {
                result = Schema.ValidateValues(item);
            }

            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count());
        }
    }

    public class ValidExpandoValuesData : IEnumerable<object[]> {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new List<ExpandoValues>() {
                    new ExpandoValues {
                         Values = {
                            { "external_id", "EX01" },
                            { "external_id_with_valid_values", "EX03" },
                            { "building_construction_date", "23-05-1980" },
                            { "property_code", 10 }
                        }
                    },
                    new ExpandoValues {
                         Values = {
                            { "external_id", "10" },
                            { "external_id_with_valid_values", "EX02" },
                            { "building_construction_date", DateTime.Now.AddYears(-5) },
                            { "property_code", 15 }
                        }
                    }
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class InValidExpandoValuesData : IEnumerable<object[]> {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new List<ExpandoValues>() {
                    new ExpandoValues {
                         Values = {
                            { "external_id", "EX0000000000001" },
                            { "external_id_with_valid_values", "EX05" },
                            { "building_construction_date", DateTime.Now },
                            { "property_code", 51 }
                        }
                    },
                    new ExpandoValues {
                         Values = {
                            { "external_id", 13298470 },
                            { "external_id_with_valid_values", "EXTERNAL-01" },
                            { "building_construction_date", "" },
                            { "property_code", 100 }
                        }
                    }
                    ,
                    new ExpandoValues {
                         Values = {
                            { "property_code", "ABC123" }
                        }
                    }
                }
            };
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
