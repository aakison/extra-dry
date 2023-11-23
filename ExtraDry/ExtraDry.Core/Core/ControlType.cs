namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ControlType
{

    BestMatch,

    SelectList,

    RadioButtons,

}
