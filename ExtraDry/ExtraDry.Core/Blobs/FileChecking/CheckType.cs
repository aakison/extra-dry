namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CheckType
{
    /// <summary>
    /// For this extension, check the filename as well as the magic bytes. This is the default
    /// behaviour
    /// </summary>
    BytesAndFilename,

    /// <summary>
    /// For this extension, don't reject based off magic bytes.
    /// </summary>
    /// <remarks>
    /// Used where a shared magic byte would reject a file you want. Eg Reject .apk, but don't
    /// reject .docx
    /// </remarks>
    FilenameOnly
}
