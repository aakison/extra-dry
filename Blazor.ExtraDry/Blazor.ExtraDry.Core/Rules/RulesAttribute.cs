#nullable enable

using System;

namespace Blazor.ExtraDry {

    /// <summary>
    /// Declare for a property to instruce the `RuleEngine` how to address the creating, updating and deleting of the property.
    /// WARNING: May cause problems with Blazor debugging, see: https://github.com/dotnet/aspnetcore/issues/25380
    /// NOTE: Issue reported fixed in .NET 6 (due Nov 2021)
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RulesAttribute : Attribute {

        public RulesAttribute()
        {

        }

        public RulesAttribute(RuleAction defaultRule = RuleAction.Allow)
        {
            UpdateAction = defaultRule;
            CreateAction = defaultRule;
        }

        public RuleAction CreateAction { get; set; } = RuleAction.Allow;

        public RuleAction UpdateAction { get; set; } = RuleAction.Allow;

        /// <summary>
        /// If set, provides a value that is applied to this property during a deletion.
        /// The `RuleEngine.Delete` method will return `true` if any properties have a DeleteValue.
        /// This should be interpreted as a soft-delete.
        /// </summary>
        public object? DeleteValue { get; set; }

    }

}
