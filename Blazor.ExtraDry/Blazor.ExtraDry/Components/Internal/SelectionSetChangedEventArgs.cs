#nullable enable

using System;
using System.Collections.Generic;

namespace Blazor.ExtraDry.Components.Internal {

    /// <summary>
    /// The event arguments used by the `SelectionSet.Changed` event handler
    /// </summary>
    public class SelectionSetChangedEventArgs : EventArgs {

        /// <summary>
        /// Indicates the type of the change.
        /// </summary>
        public SelectionSetChangedType Type { get; set; }

        /// <summary>
        /// When `Type` is `Added` or `Changed`, the list of newly selected items.
        /// </summary>
        public List<object> Added { get; } = new List<object>();

        /// <summary>
        /// When `Type` is 'Removed` or `Changed`, the list of removed items.
        /// </summary>
        public List<object> Removed { get; } = new List<object>();

    }
}
