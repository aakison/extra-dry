#nullable enable

namespace Blazor.ExtraDry {
    public enum PropertySize {

        /// <summary>
        /// A small property, such as an int field, small enough to fit four fields on a single line.
        /// </summary>
        Small = 1,

        /// <summary>
        /// A medium property, such as short text that can fit two to a line.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// A large property, usually would get it's own line but could share with a small field.
        /// </summary>
        Large = 3,

        /// <summary>
        /// An extra large property, always gets its own line.
        /// </summary>
        Jumbo = 4,

    }
}
