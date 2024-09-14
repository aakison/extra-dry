namespace Sample.Shared;

/// <summary>
/// Represents a soft-delete capable state for Templates.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TemplateState {

    /// <summary>
    /// The template is currently active and can be assigned to new companies.
    /// </summary>
    Active = 0,

    /// <summary>
    /// The template is inactive and no longer in use, but still might be attached to historical companies.
    /// </summary>
    Inactive = 1,

}
