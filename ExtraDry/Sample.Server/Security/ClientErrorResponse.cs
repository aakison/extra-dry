#nullable enable

namespace Sample.Server.Security {

    /// <summary>
    /// Wrapper for additional information about client errors (4xx status codes).
    /// </summary>
    public class ClientErrorResponse {

        /// <summary>
        /// The status code for the response.
        /// </summary>
        public int StatusCode { get; set; } = 400;

        /// <summary>
        /// A human readable description of the underlying cause for common causes of errors.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// An alternate description with less technical jargon that can be used to display to users in a last-chance exception handler.
        /// </summary>
        public string Display { get; set; } = string.Empty;

        /// <summary>
        /// A code for display to users in a last-chance exception handler to help identify specific traces during debugging.
        /// </summary>
        public string DisplayCode { get; set; } = string.Empty;
    }
}
