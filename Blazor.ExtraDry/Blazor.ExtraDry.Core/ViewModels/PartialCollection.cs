using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Blazor.ExtraDry {

    /// <summary>
    /// Represents a generic payload for returning lists of items from an API that need to be fragmented for performance.
    /// </summary>
    public class PartialCollection<T> {

        /// <summary>
        /// Create a new collection from a list of items.
        /// </summary>
        public PartialCollection(IList<T> items)
        {
            Items = items;
        }

        /// <summary>
        /// Create a new collection from an enumerable of items.
        /// </summary>
        public PartialCollection(IEnumerable<T> items)
        {
            Items = new List<T>(items);
        }

        /// <summary>
        /// The UTC date/time that the partial results were created.
        /// The client could use this as part of a caching strategy, but this is not needed by the server.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// If the full collection is a subset of all items, this is the query that was used to filter the full collection.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// If the collection is sorted, this is the name of the Property the sort is performed on.
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// The starting index of this partial collection within the full collection.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// The number of items in this partial collection.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// The total number of items in the full collection of items.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// An arbitrary token sent by the server used to managed consistency of results.
        /// As a best-practice, always send this token back to the server when fetching additional partial results,
        /// don't rely on the `Query` and `Start` indexes alone.
        /// </summary>
        public string ContinuationToken { get; set; }

        /// <summary>
        /// The actual collecton of items.  Within the full collection, these are in the position offset by `Start`.
        /// </summary>
        public IList<T> Items { get; private set; }

        /// <summary>
        /// Indicates if this partial collection is also the full collection.
        /// Typical when the collection on the server is small.
        /// </summary>
        [JsonIgnore]
        public bool IsFullCollection => Count == Total;

    }
}
