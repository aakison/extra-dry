namespace Blazor.ExtraDry {

    /// <summary>
    /// Standard payload for list controllers endpoints that return paged results, e.g. using `PartialCollection`.
    /// </summary>
    public class PartialQuery {

        /// <summary>
        /// The entity specific text to filter the collection by.
        /// This will typically match across multiple properties or even access a full text index.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// If the request would like sorted results, the name of the property to sort by.
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Indicates if the results are requested in ascending order by `Sort`.
        /// </summary>
        public bool Ascending { get; set; }

        /// <summary>
        /// The number of records to skip before returning results.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// The requested number of records to take.  
        /// Actual result might be less based on available records or endpoint limitations.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// The continuation token from the previous response.
        /// When provided, this will override other options such as `Sort` and `Filter`, but not `Skip` and `Take`.
        /// </summary>
        public string Token { get; set; }
    }

}
