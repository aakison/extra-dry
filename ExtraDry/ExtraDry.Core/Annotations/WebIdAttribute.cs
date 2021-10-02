#nullable enable

using System.ComponentModel.DataAnnotations;

namespace ExtraDry.Core {

    /// <summary>
    /// Validates that a string field is a valid WebId, allowing it to be part of a URI path without translation.
    /// This only allows letters, numbers, and the punctuation "-", "_", ".", and "~".
    /// </summary>
    public class WebIdAttribute : RegularExpressionAttribute {

        /// <summary>
        /// Indicates string is a WebId.
        /// </summary>
        public WebIdAttribute() : base(WebIdRegex)
        {
            ErrorMessage = WebIdErrorMessage;
        }

        private const string WebIdErrorMessage = @"WebId should be generated and use only letters and hyphens.";

        /// <summary>
        /// The regex for a valid WebId.
        /// </summary>
        /// <remarks>
        /// Limits to 100 characters, but _should_ be smaller, also use with MaxLength the ensure smaller size.
        /// </remarks>
        public const string WebIdRegex = @"^[a-zA-Z0-9-_~\.]{0,100}$";

    }
}
