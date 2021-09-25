#nullable enable

namespace ExtraDry.Core {

    /// <summary>
    /// Determines the scope for blob, which affects which container it's in and there for the security level.
    /// </summary>
    public enum BlobScope {

        /// <summary>
        /// Private scope, a SAS token will be required.
        /// </summary>
        Private = 0,

        /// <summary>
        /// Public scope, unsecured, e.g. logos
        /// </summary>
        Public = 1,

    }
}
