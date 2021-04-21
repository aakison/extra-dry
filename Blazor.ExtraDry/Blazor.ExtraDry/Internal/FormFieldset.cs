#nullable enable

using System.Collections.Generic;

namespace Blazor.ExtraDry.Internal {
    internal class FormFieldset {

        public string Legend { get; set; } = string.Empty;

        public ColumnType Column { get; set; } = ColumnType.Primary;

        public List<FormLine> Lines { get; } = new();

    }
}
