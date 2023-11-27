using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ExtraDry.UploadTools {

    /// <summary>
    /// Defines a file type that can be checked for when uploading a file.
    /// </summary>
    public class FileTypeDefinition {
        /// <summary>
        /// A human readable description of the file type eg. "Portable Network Graphics Image"
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The file extensions that are associated with this file type. eg ["jpg", "jpeg"]
        /// </summary>
        public List<string> Extensions { get; set; } = new List<string>();

        /// <summary>
        /// The mime types that are associated with this file type. eg ["application/xml"]
        /// </summary>
        public List<string> MimeTypes { get; set; } = new List<string>();

        /// <summary>
        /// An object defining a way that this file type can be identified by its content.
        /// </summary>
        public List<MagicBytes> MagicBytes { get; set; } = new List<MagicBytes>();
    }

    public class MagicBytes {
        /// <summary>
        /// The offset from the start of the file that these bytes can be found
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// The value to search for to define this file type. How this is interpreted is defined by the Type property
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Defines how to search for the Value in the provided file
        /// - If the value is "Bytes", the value is interpreted as a hex-string and that set of bytes is searched for Eg. a java class starting with the bytes represented by the hex string "cafebabe"
        /// - If the value is "Content", the value is interpreted from the content as UTF8, and that set of bytes is searched for. Eg. a Post script file starting with "%!"
        /// </summary>
        public ByteType Type { get; set; }

        /// <summary>
        /// Gets the magic byte value represented as a byte[] that can be searched for within a file
        /// </summary>
        [JsonIgnore]
        public byte[] ValueAsByte {
            get {
                if(valueAsByte != null) { return valueAsByte; }
                if(Type == ByteType.Bytes) {
                    valueAsByte = HexStringHelper.GetBytesFromString(Value);
                } else {
                    valueAsByte = UTF8Encoding.UTF8.GetBytes(Value);
                }
                return ValueAsByte;
            }
        }
        private byte[] valueAsByte;
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ByteType {
        /// <summary>
        /// This magic byte type is a value that is searched for within a file. This value will be interpreted as UTF8 and those bytes searched for in the file
        /// </summary>
        Content,

        /// <summary>
        /// This magic byte type is a specific sequence of bytes that is best described as a hex string.
        /// </summary>
        Bytes,
    }
}
