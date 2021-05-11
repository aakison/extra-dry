#nullable enable

namespace Blazor.ExtraDry {

    /// <summary>
    /// Represents a basic query to filter against a list of items.
    /// </summary>
    public class FilterQuery {

        /// <summary>
        /// The entity specific text to filter the collection by.
        /// This will typically match across multiple properties or even access a full text index.
        /// </summary>
        public string Filter { get; set; } = string.Empty;

        /// <summary>
        /// If the request would like sorted results, the name of the property to sort by.
        /// </summary>
        public string Sort { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the results are requested in ascending order by `Sort`.
        /// </summary>
        public bool Ascending { get; set; }

    }
}
