using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Components.Internal {
    internal class ItemCollection<T> : List<ListItemInfo<T>> {

        public ItemCollection()
        {
        }

        public void AddRange(ICollection<T> source)
        {
            if(source != null) {
                AddRange(source.Select(e => new ListItemInfo<T> { Item = e }));
            }
        }

        public void SortBy(PropertyDescription property)
        {
            
        }

    }
}
