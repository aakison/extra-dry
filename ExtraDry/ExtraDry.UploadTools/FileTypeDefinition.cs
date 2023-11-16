using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ExtraDry.UploadTools {
    public class FileTypeDefinition {
        public string Description { get; set; }

        public List<string> Extensions { get; set; } = new List<string>();

        public List<string> MimeTypes { get; set; } = new List<string>();

        public List<MagicBytes> MagicBytes { get; set; } = new List<MagicBytes>();
    }

    public class MagicBytes {
        public int Offset { get; set; }
        public string Value { get; set; }
        public ByteType Type { get; set; }

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
        Content,
        Bytes,
    }
}
