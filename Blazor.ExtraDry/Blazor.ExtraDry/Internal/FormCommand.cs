#nullable enable

namespace Blazor.ExtraDry.Internal {

    /// <summary>
    /// Within a form indicates the type of command that is expected.
    /// The DryForm will use this to inject form management buttons like adding new rows or re-ordering items.
    /// </summary>
    public enum FormCommand {

        AddNew,

    }
}
