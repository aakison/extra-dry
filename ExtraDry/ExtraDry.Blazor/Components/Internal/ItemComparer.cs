#nullable enable

using System;
using System.Collections.Generic;

namespace ExtraDry.Blazor.Components.Internal {
    internal class ItemComparer<T> : IComparer<ListItemInfo<T>> {

        public ItemComparer(PropertyDescription property, bool ascending)
        {
            Property = property;
            Ascending = ascending;
            scale = ascending ? 1 : -1;
            propertyIsEnum = property.HasDiscreteValues;
        }

        public PropertyDescription Property { get; }

        public bool Ascending { get; }

        private readonly int scale;

        private readonly bool propertyIsEnum;

        /// <summary>
        /// Compare the two elements using the `Property` and sort order.
        /// </summary>
        /// <remarks>
        /// Null handling: https://stackoverflow.com/questions/17025900/override-compareto-what-to-do-with-null-case
        /// </remarks>
        public int Compare(ListItemInfo<T>? x, ListItemInfo<T>? y)
        {
            if(x == null && y == null) {
                return 0;
            }
            else if(x == null) {
                return 1;
            }
            else if(y == null) {
                return -1;
            }
            if(propertyIsEnum) {
                var xValue = Property.GetDiscreteSortOrder(x.Item);
                var yValue = Property.GetDiscreteSortOrder(y.Item);
                return scale * xValue.CompareTo(yValue);
            }
            else {
                var xValue = Property.GetValue(x.Item) as IComparable;
                var yValue = Property.GetValue(y.Item) as IComparable;
                return scale * (xValue?.CompareTo(yValue) ?? 0);
            }
        }
    }
}
