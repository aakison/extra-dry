#nullable enable

using System.Collections.ObjectModel;

namespace Blazor.ExtraDry.Internal {
    /// <summary>
    /// Indicates a logical grouping of properties that can occur on a single line in a form.
    /// Some `FormLine`s will contain a single property or a single header.
    /// Others will contain several short properties that can be stacked together.
    /// </summary>
    internal class FormLine {

        public FormLine(object model)
        {
            Model = model;
        }

        public Collection<DryProperty> FormProperties { get; } = new();

        public object Model { get; set; }

    }
}
