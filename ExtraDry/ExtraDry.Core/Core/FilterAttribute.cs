using System;

namespace ExtraDry.Core {

    /// <summary>
    /// Defines a property that can be used with `FilterQuery` and the `PartialQueryable` extensions to `IQueryable`.
    /// WARNING: May cause problems with Blazor debugging, see: https://github.com/dotnet/aspnetcore/issues/25380
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterAttribute : Attribute {

        public FilterAttribute(FilterType type = FilterType.Contains)
        {
            Type = type;
        }

        public FilterType Type { get; set; }

    }

}
