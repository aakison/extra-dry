namespace ExtraDry.Server
{
    /// <summary>
    /// Defines the behaviour of ExtraDry when providing a sort method to an underlying data store.
    /// </summary>
    public enum SortStabilization
    {
        /// <summary>
        ///  The default behaviour, this will use a secondary property on sorts to provide consistent results
        /// </summary>
        AlwaysAddKey = 0,

        /// <summary>
        /// When retrieving list from a data store this will not use a secondary column to provide a stabilized sort
        /// This is useful for cases where your data store may not support sorting by 2 columns simultaneously.
        /// For an unsorted list, this will use the provider defaults to stabilize the returned list
        /// </summary>
        ProviderDefaultsOnly = 1,

        /// <summary>
        /// When retrieving list from a data store this will not use a secondary column to provide a stabilized sort
        /// This is useful for cases where your data store may not support sorting by 2 columns simultaneously.
        /// For an unsorted list, this will use the models Key property to stabilize the sort.
        /// </summary>
        AddKeyWhenUnsorted = 2
    }
}
