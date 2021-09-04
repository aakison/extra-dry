namespace ExtraDry.Core {

    /// <summary>
    /// Represents a basic query to filter against a list of items.
    /// </summary>
    public class FilterQuery {

        /// <summary>
        /// The entity specific text to filter the collection by.
        /// This will typically match across multiple properties or even access a full text index.
        /// </summary>
        public string? Filter { get; set; } 

        /// <summary>
        /// If the request would like sorted results, the name of the property to sort by.
        /// </summary>
        public string? Sort { get; set; }

        /// <summary>
        /// A property that is used to break sorting ties, should be unique and 
        /// ideally monotonically increasing.
        /// </summary>
        public string? Stabilizer { get; set; }

        /// <summary>
        /// Indicates if the results are requested in ascending order by `Sort`.
        /// </summary>
        public bool Ascending { get; set; }

    }
}
