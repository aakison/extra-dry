using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraDry.Blazor {
    public interface IGroupProvider<T> {

        public T GetGroup(T item);

        public string GroupColumn { get; }

    }
}
