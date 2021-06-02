namespace Sample.Shared {

    /// <summary>
    /// Represents a soft-delete capable state for Services which indicate what services companies can provide.
    /// </summary>
    public enum ServiceState {

        Unknown = 0,

        Pending = 1,

        Active = 2,

        Inactive = 3,

    }
}
