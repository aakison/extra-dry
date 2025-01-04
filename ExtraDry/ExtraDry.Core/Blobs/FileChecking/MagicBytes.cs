using ExtraDry.Core.Internal;
using System.Text;

namespace ExtraDry.Core;

public class MagicBytes
{
    /// <summary>
    /// The offset from the start of the file that these bytes can be found
    /// </summary>
    public int Offset { get; set; }

    /// <summary>
    /// The value to search for to define this file type. How this is interpreted is defined by the
    /// Type property
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Defines how to search for the Value in the provided file
    /// - If the value is "Bytes", the value is interpreted as a hex-string and that set of bytes
    /// is searched for Eg. a java class starting with the bytes represented by the hex string
    /// "cafebabe"
    /// - If the value is "Content", the value is interpreted from the content as UTF8, and that
    /// set of bytes is searched for. Eg. a Post script file starting with "%!"
    /// </summary>
    public MagicByteType Type { get; set; }

    /// <summary>
    /// Gets the magic byte value represented as a byte[] that can be searched for within a file
    /// </summary>
    [JsonIgnore]
    public byte[] ValueAsByte {
        get {
            if(valueAsByte != null) { return valueAsByte; }
            if(Value == null) {
                throw new ArgumentException("Provided magic bytes do not have a value defined");
            }
            valueAsByte = Type == MagicByteType.Bytes
                ? HexStringHelper.GetBytesFromString(Value)
                : Encoding.UTF8.GetBytes(Value);
            return ValueAsByte;
        }
    }

    private byte[]? valueAsByte;
}
