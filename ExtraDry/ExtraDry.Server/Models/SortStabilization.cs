namespace ExtraDry.Server
{
    /// <summary>
    /// Defines the behaviour of ExtraDry when providing a sort method to an underlying data store.
    /// </summary>
    public enum SortStabilization
    {
        /// <summary>
        ///  The default behaviour, this will use a secondary property on sorts to provide consistent results.
        ///  This is the default becuase it will produce stable lists in all persistence providers where sorting by multiple columns is allowed. 
        ///  It is only exceptions to this sorting being allowed that this will likely be changed
        /// </summary>
        AlwaysAddKey,

        /// <summary>
        /// When retrieving list from a data store this will not use a secondary column to provide a stabilized sort
        /// This is useful for cases where your data store may not support sorting by 2 columns simultaneously.
        /// For an unsorted list, this will use the provider defaults to stabilize the returned list
        /// </summary>
        ProviderDefaultsOnly,

        /// <summary>
        /// When retrieving list from a data store this will not use a secondary column to provide a stabilized sort
        /// This is useful for cases where your data store may not support sorting by 2 columns simultaneously.
        /// For an unsorted list, this will use the models Key property to stabilize the sort.
        /// </summary>
        AddKeyWhenUnsorted
    }
}
