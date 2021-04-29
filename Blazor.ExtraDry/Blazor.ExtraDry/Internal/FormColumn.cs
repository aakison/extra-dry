#nullable enable

using System.Collections.Generic;

namespace Blazor.ExtraDry.Internal {
    /// <summary>
    /// A column is a collection of fieldsets that might break into columns on wide screens.
    /// </summary>
    internal class FormColumn {

        public ColumnType Type { get; set; }

        public string ClassName => Type.ToString().ToLowerInvariant();

        public List<FormFieldset> Fieldsets { get; } = new();

    }
}
