#nullable enable

using System;

namespace Blazor.ExtraDry {

    /// <summary>
    /// Place with the first property in a group to provide logical separation between groups of properties.
    /// In the UI, this will create forms that have headers between properties.
    /// Additionally, use the `Column` attribute to suggest which column the properties should be placed in on wide screens.
    /// WARNING: May cause problems with Blazor debugging, see: https://github.com/dotnet/aspnetcore/issues/25380
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderAttribute : Attribute {

        /// <summary>
        /// Create a new header with the specified title.
        /// </summary>
        public HeaderAttribute(string title)
        {
            Title = title;
        }

        /// <summary>
        /// The title that is displayed, typically in an HTML fieldset legend.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The preferred column for the group of properties.
        /// May be ignored by layout depending on screen width.
        /// </summary>
        public ColumnType Column { get; set; } = ColumnType.Primary;

        /// <summary>
        /// The optional description that is placed on the header in the form.
        /// </summary>
        public string? Description { get; set; } 
    }
}
