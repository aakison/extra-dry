#nullable enable

using System;
using System.Collections.Generic;

namespace Blazor.ExtraDry {

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
        public string Filter { get; set; } = string.Empty;

        /// <summary>
        /// If the collection is sorted, this is the name of the Property the sort is performed on.
        /// </summary>
        public string Sort { get; set; } = string.Empty;

        /// <summary>
        /// For sorted collections, the stabalizer resolves ties consistently.
        /// Choose a unique monotonically increasing value such as Id or CreatedDate, not typically shown to users.
        /// </summary>
        public string Stabalizer { get; set; } = string.Empty;

        /// <summary>
        /// The total number of items in the full collection of items.
        /// </summary>
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
