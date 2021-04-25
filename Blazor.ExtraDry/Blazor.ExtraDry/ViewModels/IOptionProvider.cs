using Microsoft.AspNetCore.Components.Web.Virtualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public interface IOptionProvider<T> {

        ValueTask<ItemsProviderResult<T>> GetItemsAsync(CancellationToken token = default);

    }
}
