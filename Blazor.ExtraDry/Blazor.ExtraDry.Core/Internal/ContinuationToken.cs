#nullable enable

using System;
using System.IO;

namespace Blazor.ExtraDry {

    /// <summary>
    /// Represents a token that can be used to help keeep pages of results in a stable order when calling APIs.
    /// </summary>
    /// <remarks>
    /// This class is kept internal as the actual contents shouldn't leak to consumers.
    /// It's not security critcal, but want to discourage any token hacking so that future versions aren't breaking changes.
    /// </remarks>
    internal class ContinuationToken {

        public ContinuationToken()
        {

        }

        public ContinuationToken(string filter, string sort, bool ascending, string stabalizer, int skip, int take, ContinuationToken? previous)
        {
            // Use token, fallback to explicit values...
            Filter = previous?.Filter ?? filter;
            Sort = previous?.Sort ?? sort;
            Ascending = previous?.Ascending ?? ascending;
            Stabalizer = previous?.Stabalizer ?? stabalizer;
            // Use explicit values, fallback to token...
            Take = ActualTake(previous, take);
            Skip = ActualSkip(previous, skip) + Take;
        }

        public string Filter { get; set; } = string.Empty;

        public string Sort { get; set; } = string.Empty;

        public bool Ascending { get; set; }

        public string Stabalizer { get; set; } = string.Empty;

        public int Skip { get; set; }

        public int Take { get; set; }

        public override string ToString()
        {
            using var memory = new MemoryStream(); 
            using var writer = new BinaryWriter(memory); 
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

        public static ContinuationToken? FromString(string token)
        {
            if(string.IsNullOrWhiteSpace(token)) {
                return default;
            }
            // One item cache to make this a lookup on token to help keep this internal.
            if(token == lastTokenString) {
                return lastToken;
            }
            var bytes = Convert.FromBase64String(token);
            using var memory = new MemoryStream(bytes); 
            using var reader = new BinaryReader(memory); 
            var result = new ContinuationToken {
                Filter = reader.ReadString(),
                Sort = reader.ReadString(),
                Ascending = reader.ReadBoolean(),
                Stabalizer = reader.ReadString(),
                Skip = reader.ReadInt32(),
                Take = reader.ReadInt32(),
            };
            lastTokenString = token;
            lastToken = result;
            return result;
        }

        private static string lastTokenString = string.Empty;

        private static ContinuationToken? lastToken;

        /// <summary>
        /// Returns the winning `take` amount where API call can override token, but not make it 0.
        /// </summary>
        internal static int ActualTake(ContinuationToken? token, int take)
        {
            if(take > 0) {
                return take;
            }
            else {
                return token?.Take ?? PageQuery.DefaultTake;
            }
        }

        /// <summary>
        /// Returns the winning `skip` amount where API call can override token (but both must agree to make it zero).
        /// </summary>
        internal static int ActualSkip(ContinuationToken? token, int skip) {
            if(skip > 0) {
                return skip;
            }
            else {
                return token?.Skip ?? 0;
            }
        }
    }
}
