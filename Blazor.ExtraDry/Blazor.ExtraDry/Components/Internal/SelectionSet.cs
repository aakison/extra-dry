#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.ExtraDry.Components.Internal {

    /// <summary>
    /// Represents a list of items that can be selected, supporting both single-select and multi-select lists.
    /// Provides an efficient mechanism for the common selecting scenarios in the user interface.
    /// </summary>
    /// <remarks>
    /// This seemingly simple class has a (nearly) too complex implementation.  The backing unit tests are
    /// critical to its operation.  The cause is the implementation of the `SelectAll`, where the selection
    /// extends to items that are possibly virtual and not downloaded to the current list view.
    /// This is done by implementing both the obvious "inclusive" list of items as well as the less obvious 
    /// (and harder to debug) "exclusive" list of items.  In the exclusive mode, the list of un-checked
    /// items is stored.  In a set of 10,000 items this aligns with a human's typical use-case of "select a few" 
    /// or "select all but a few".  The worst case would be 5,000 selected and 5,000 unselected.
    /// </remarks>
    public class SelectionSet {

        public void Clear()
        {
            if(!inclusiveStorage || items.Any()) {
                items.Clear();
                inclusiveStorage = true;
                var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.Cleared };
                Changed?.Invoke(this, args);
            }
        }

        public void Add(object item)
        {
            if((ExclusiveStorage && !items.Contains(item)) || (!ExclusiveStorage && items.Contains(item))) {
                return;
            }
            var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.Added } ;
            if(MultipleSelect) {
                args.Added.Add(item);
                if(inclusiveStorage) {
                    items.Add(item);
                }
                else {
                    items.Remove(item);
                }
            }
            else {
                args.Removed.AddRange(items);
                if(items.Any()) {
                    args.Type = SelectionSetChangedType.Changed;
                    items.Clear();
                }
                args.Added.Add(item);
                items.Add(item);
            }
            Changed?.Invoke(this, args);
        }

        public void Remove(object item)
        {
            if(!MultipleSelect && !items.Contains(item)) {
                return;
            }
            if(MultipleSelect && inclusiveStorage && !items.Contains(item)) {
                return;
            }
            if(ExclusiveStorage && items.Contains(item)) {
                return;
            }
            var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.Removed };
            args.Removed.Add(item);
            if(MultipleSelect && !inclusiveStorage) {
                items.Add(item);
            }
            else {
                items.Remove(item);
            }
            Changed?.Invoke(this, args);
        }

        public void SelectAll()
        {
            if(ExclusiveStorage && !items.Any()) {
                return;
            }
            if(MultipleSelect) {
                items.Clear();
                inclusiveStorage = false;
            }
            else {
                throw new InvalidOperationException("Can't perform SelectAll() on a single selection set.");
            }
            var args = new SelectionSetChangedEventArgs() { Type = SelectionSetChangedType.SelectAll };
            Changed?.Invoke(this, args);
        }

        public bool Contains(object item) => ExclusiveStorage ? !items.Contains(item) : items.Contains(item);

        public bool Any() => ExclusiveStorage || items.Any();

        /// <summary>
        /// Indicates if a single selection is made, independent of whether multiple or single select mode is on.
        /// </summary>
        public bool Single() => (!MultipleSelect || inclusiveStorage) && items.Count == 1;

        public bool All() => ExclusiveStorage && !items.Any();

        public IEnumerable<object> Items => items.AsEnumerable(); // TODO: Make function that can optionally supply super-set?

        public bool MultipleSelect { get; set; }


        public event EventHandler<SelectionSetChangedEventArgs>? Changed;


        private bool inclusiveStorage = true;

        private bool ExclusiveStorage => MultipleSelect && !inclusiveStorage;

        private readonly List<object> items = new();

        public static SelectionSet? Lookup(object key) => key == null ? null : registered.ContainsKey(key) ? registered[key] : null;

        public static SelectionSet Register(object key) {
            if(!registered.ContainsKey(key)) {
                registered.Add(key, new SelectionSet());
            }
            return registered[key];
        }

        public static void Deregister(object key)
        {
            registered.Remove(key);
        }

        private readonly static Dictionary<object, SelectionSet> registered = new();

    }

}
