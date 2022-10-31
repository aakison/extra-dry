namespace ExtraDry.Core {

    /// <summary>
    /// The processing rule to be applied to a property when a `FilterQuery` or `PageQuery` has a `Filter` provided.
    /// </summary>
    public enum FilterType {

        /// <summary>
        /// Performs an exact match of the value, the default behavior.
        /// </summary>
        Equals = 0,

        /// <summary>
        /// If the property is a `string`, then the filter matches when the text matches the start of the property.
        /// </summary>
        StartsWith,

        /// <summary>
        /// If the property is a `string`, then the filter matches when the text occurs anywhere within the property.
        /// </summary>
        Contains,

    }

}
