namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Within a form indicates the type of command that is expected. The DryForm will use this to
/// inject form management buttons like adding new rows or re-ordering items.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FormCommand
{
    AddNew,
}
