using System;

namespace Blazor.ExtraDry {

    [AttributeUsage(AttributeTargets.Property)]
    public class HeaderAttribute : Attribute {

        public HeaderAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; set; }

        public ColumnType Column { get; set; } = ColumnType.Primary;

        /// <summary>
        /// The optional description that is placed on the header in the form.
        /// </summary>
        public string Description { get; set; } 
    }
}
