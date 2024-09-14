namespace ExtraDry.Core;

/// <summary>
/// Determines which characters sets are allowed in filenames during the cleaning process.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilenameCharacters
{
    /// <summary>
    /// All characters are acceptable and the filename will not be cleaned.
    /// </summary>
    All,

    /// <summary>
    /// Only Unicode alphanumeric characters are allowed.  This is the default for the names of files.
    /// </summary>
    UnicodeAlphaNumeric,

    /// <summary>
    /// Only Ascii alphanumeric characters are allowed.  This is the default for the extensions of files.
    /// </summary>
    AsciiAlphaNumeric,

}
