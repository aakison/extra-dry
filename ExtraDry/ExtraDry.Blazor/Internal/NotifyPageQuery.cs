using ExtraDry.Core;
using System;

namespace ExtraDry.Blazor.Internal;

// Might a MessageBus be better?  Internal so consider and change if desired.
internal class NotifyPageQuery : PageQuery {

    public event EventHandler? OnChanged;

    public void NotifyChanged()
    {
        OnChanged?.Invoke(this, EventArgs.Empty);
    }

}
