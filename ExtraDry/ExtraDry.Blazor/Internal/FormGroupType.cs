namespace ExtraDry.Blazor.Internal;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FormGroupType
{
    Properties,

    Objects,

    Element,

    Commands,
}
