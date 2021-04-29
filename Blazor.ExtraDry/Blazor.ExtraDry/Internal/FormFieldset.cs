﻿#nullable enable

using System.Collections.Generic;

namespace Blazor.ExtraDry.Internal {

    /// <summary>
    /// Represents an HTML fieldset, visually boxing groups of input controls.
    /// </summary>
    internal class FormFieldset {

        public string Legend { get; set; } = string.Empty;

        public ColumnType Column { get; set; } = ColumnType.Primary;

        public List<FormGroup> Groups { get; } = new();

    }
}