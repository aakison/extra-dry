namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WarehouseBehavior
{
    None,

    AdditiveMeasure,

    SemiAdditiveMeasure,

    NonAdditiveMeasure,

    DimensionAttribute,
}
