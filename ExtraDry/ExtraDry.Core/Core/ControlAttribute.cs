#nullable enable

using System;

namespace ExtraDry.Core {

    /// <summary>
    /// Expands the displays options for a property when it is displayed as a control.
    /// Not typically required, but use to get enhanced controls, such as a Radio Button list instead of a select dropdown.
    /// WARNING: May cause problems with Blazor debugging, see: https://github.com/dotnet/aspnetcore/issues/25380
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ControlAttribute : Attribute {

        public ControlAttribute(ControlType type = ControlType.BestMatch)
        {
            Type = type;
        }

        public ControlType Type { get; set; }

        public string IconTemplate { get; set; } = string.Empty;

        public string CaptionTemplate { get; set; } = "{0}";

    }

}
