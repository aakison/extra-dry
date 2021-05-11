#nullable enable

using System;
using System.Collections.Generic;

namespace Blazor.ExtraDry {

    public class FilteredCollection<T> {

        /// <summary>
        /// Create a new collection from a list of items.
        /// </summary>
        public FilteredCollection(IList<T> items)
        {
            Items = items;
        }

        /// <summary>
        /// Create a new collection from an enumerable of items.
        /// </summary>
        public FilteredCollection(IEnumerable<T> items)
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
        public string Filter { get; set; } = string.Empty;

        /// <summary>
        /// If the collection is sorted, this is the name of the Property the sort is performed on.
        /// </summary>
        public string Sort { get; set; } = string.Empty;

        /// <summary>
        /// The total number of items in the full collection of items.
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// The actual collecton of items.  Within the full collection, these are in the position offset by `Start`.
        /// </summary>
        public IList<T> Items { get; private set; }

    }
}
