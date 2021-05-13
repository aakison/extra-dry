#nullable enable

using System;

namespace Blazor.ExtraDry {

    [AttributeUsage(AttributeTargets.Property)]
    public class FilterAttribute : Attribute {

        public FilterAttribute(FilterType type = FilterType.Contains)
        {
            Type = type;
        }

        public FilterType Type { get; set; }

    }

}
