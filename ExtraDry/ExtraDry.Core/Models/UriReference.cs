#nullable enable

using System;

namespace ExtraDry.Core.Models {

    /// <summary>
    /// A simple container for holding a URI Identifier, such as returned from a Create method.
    /// </summary>
    public class UriReference {

        /// <summary>
        /// Create with mandatory URI.
        /// </summary>
        public UriReference(Uri uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// The unique URI identifier.
        /// </summary>
        public Uri Uri { get; }

    }

}
