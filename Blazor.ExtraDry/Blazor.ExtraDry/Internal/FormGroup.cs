#nullable enable

using System.Collections.Generic;

namespace Blazor.ExtraDry.Internal {
    /// <summary>
    /// Represents a logical group of lines inside a form.
    /// These might be grouped for different reasons as specifie by the `Type` property.
    /// </summary>
    internal class FormGroup {

        public FormGroup(object target)
        {
            Target = target;
        }

        public FormGroupType Type { get; set; } = FormGroupType.Properties;

        public string ClassName => Type.ToString().ToLowerInvariant();

        public List<FormLine> Lines { get; } = new();

        public object Target { get; set; }

    }
}
