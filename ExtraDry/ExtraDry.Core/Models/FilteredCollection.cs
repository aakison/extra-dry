using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExtraDry.Core {

    /// <summary>
    /// Represents a generic payload for returning lists of items from an API.
    /// </summary>
    public class FilteredCollection<T> {

        /// <summary>
        /// The UTC date/time that the partial results were created.
        /// The client could use this as part of a caching strategy, but this is not needed by the server.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// If the full collection is a subset of all items, this is the query that was used to filter the full collection.
        /// </summary>
        /// <example>term property:value</example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Filter { get; set; }

        /// <summary>
        /// If the collection is sorted, this is the name of the Property the sort is performed on.
        /// </summary>
        /// <example>property</example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Sort { get; set; }

        /// <summary>
        /// The total number of items in the full collection of items.
        /// </summary>
        /// <example>1204</example>
        public int Count => Items.Count;

        /// <summary>
        /// The actual collecton of items.  Within the full collection, these are in the position offset by `Start`.
        /// </summary>
        /// <remarks>
        /// Urge to make private setter is strong, but breaks System.Text.Json...
        /// </remarks>
        public IList<T> Items { get; set; } = new List<T>();

    }
}
