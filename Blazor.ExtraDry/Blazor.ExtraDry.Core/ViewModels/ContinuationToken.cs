using System;
using System.IO;

namespace Blazor.ExtraDry {

    /// <summary>
    /// Represents a token that can be used to help keeep pages of results in a stable order when calling APIs.
    /// </summary>
    public class ContinuationToken {

        public ContinuationToken()
        {

        }

        public ContinuationToken(string filter, string sort, bool? ascending, string stabalizer, int? skip, int? take, ContinuationToken previous)
        {
            // Use token, fallback to explicit values...
            Filter = previous?.Filter ?? filter ?? string.Empty;
            Sort = previous?.Sort ?? sort ?? string.Empty;
            Ascending = previous?.Ascending ?? ascending ?? true;
            Stabalizer = previous?.Stabalizer ?? stabalizer ?? string.Empty;
            // Use explicit values, fallback to token...
            Skip = skip ?? (previous?.Skip + previous?.Take) ?? 0;
            Take = take ?? (previous?.Take) ?? 100;
        }

        public string Filter { get; set; } = string.Empty;

        public string Sort { get; set; } = string.Empty;

        public bool Ascending { get; set; }

        public string Stabalizer { get; set; } = string.Empty;

        public int Skip { get; set; }

        public int Take { get; set; }

        public override string ToString()
        {
            using(var memory = new MemoryStream()) {
                using(var writer = new BinaryWriter(memory)) {
                    writer.Write(Filter);
                    writer.Write(Sort);
                    writer.Write(Ascending);
                    writer.Write(Stabalizer);
                    writer.Write(Skip);
                    writer.Write(Take);
                    var bytes = memory.ToArray();
                    var base64 = Convert.ToBase64String(bytes);
                    return base64;
                }
            }
        }

        public static ContinuationToken FromString(string token)
        {
            if(string.IsNullOrWhiteSpace(token)) {
                return null;
            }
            var bytes = Convert.FromBase64String(token);
            using(var memory = new MemoryStream(bytes)) {
                using(var reader = new BinaryReader(memory)) {
                    var result = new ContinuationToken {
                        Filter = reader.ReadString(),
                        Sort = reader.ReadString(),
                        Ascending = reader.ReadBoolean(),
                        Stabalizer = reader.ReadString(),
                        Skip = reader.ReadInt32(),
                        Take = reader.ReadInt32(),
                    };
                    return result;
                }
            }
        }
    }
}
