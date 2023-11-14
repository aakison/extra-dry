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
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ByteType {
        Content,
        Bytes,

    }
}
