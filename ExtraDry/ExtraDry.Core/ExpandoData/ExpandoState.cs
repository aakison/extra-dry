namespace ExtraDry.Core;

/// <summary>
/// Represents a State for an ExpandoField.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpandoState
{
    /// <summary>
    /// The field is in initial stages of creation.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// The field is currently active and user input is allowed.
    /// </summary>
    Active = 1,

    /// <summary>
    /// The field has been archived and is currently not allowed for user input.
    /// </summary>
    Archived = 2,
}
