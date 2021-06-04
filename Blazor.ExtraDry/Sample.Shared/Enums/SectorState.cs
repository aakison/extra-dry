namespace Sample.Shared {

    /// <summary>
    /// Represents a soft-delete capable state for Sectors which indicate what services companies can provide.
    /// </summary>
    public enum SectorState {

        Unknown = 0,

        Pending = 1,

        Active = 2,

        Inactive = 3,

    }
}
