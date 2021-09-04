#nullable enable

using System.Collections.Generic;

namespace ExtraDry.Blazor.Components.Internal {
    internal class GroupComparer<T> : IComparer<ListItemInfo<T>> {

        /// <summary>
        /// Creats a comparison that sorts items so they underneath the groups.
        /// Within the groups, a stable (but non-meaningful) secondary sort is used.
        /// </summary>
        public GroupComparer()
        {
            innerComparer = new HashCodeComparer();
        }

        /// <summary>
        /// Creats a comparison that sorts items so they underneath the groups.
        /// Within the groups, the specified sort order is performed.
        /// </summary>
        public GroupComparer(IComparer<ListItemInfo<T>> sortWithinGroupsComparer)
        {
            innerComparer = sortWithinGroupsComparer;
        }

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
            // Move up groupings until find common level to compare at.
            while(x.GroupDepth > y.GroupDepth && x.Group != null) {
                x = x.Group;
            }
            if(x == y) {
                // x is direct descendent of y, therefore x comes after y.
                return 1;
            }
            while(y.GroupDepth > x.GroupDepth && y.Group != null) {
                y = y.Group;
            }
            if(x == y) {
                // y is direct descendent of x, therefore x comes before y.
                return -1;
            }
            // Move up in pairs to find common ancestor
            while(x?.Group != y?.Group) {
                x = x?.Group;
                y = y?.Group;
            }
            return innerComparer.Compare(x, y);
        }

        private readonly IComparer<ListItemInfo<T>> innerComparer;

        private class HashCodeComparer : IComparer<ListItemInfo<T>> {
            public int Compare(ListItemInfo<T>? x, ListItemInfo<T>? y)
            {
                var xHash = x?.GetHashCode() ?? 0;
                var yHash = y?.GetHashCode() ?? 0;
                return xHash.CompareTo(yHash);
            }
        }

    }
}
