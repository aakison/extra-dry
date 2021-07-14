#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.ExtraDry {

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

    public enum ControlType {

        BestMatch,

        SelectList,

        RadioButtons,

    }
}
