namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FieldAccess {
    /// <summary>
    /// The property is read-only, values are ignored during creation and changes are blocked on update.
    /// </summary>
    ReadOnly,

    /// <summary>
    /// The property is read-write, values are allowed on creation and update.
    /// </summary>
    ReadWrite,

    /// <summary>
    /// The property is write-on-create, values are allowed on creation but blocked on update.
    /// </summary>
    WriteOnCreate,

    /// <summary>
    /// The property is computed and/or system generated, values are ignored during creation and update.
    /// </summary>
    Computed,
}
