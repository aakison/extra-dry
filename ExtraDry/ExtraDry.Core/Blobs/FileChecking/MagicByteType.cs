namespace ExtraDry.Core;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MagicByteType
{
    /// <summary>
    /// This magic byte type is a value that is searched for within a file. This value will be
    /// interpreted as UTF8 and those bytes searched for in the file
    /// </summary>
    Content,

    /// <summary>
    /// This magic byte type is a specific sequence of bytes that is best described as a hex
    /// string.
    /// </summary>
    Bytes,
}
