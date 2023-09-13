using System.Collections.ObjectModel;

namespace ExtraDry.Core {

    /// <summary>
    /// A collection of valid values that supports List and Linq operations.
    /// </summary>
    /// <remarks>
    /// This is a shallow wrapper to provide some future-proofing.
    /// </remarks>
    public class ValidValueCollection : Collection<ValidValue> {

        public ValidValueCollection Clone()
        {
            return Clone(this);
        }

        public static ValidValueCollection Clone(ValidValueCollection existingCollection)
        {
            if(existingCollection == null) {
                throw new ArgumentNullException("existingCollection");
            }
            var validValueCollection = new ValidValueCollection();

            // Deep copy the collection instead of copying the reference with MemberwiseClone()
            foreach(var validValue in existingCollection) {
                validValueCollection.Add(validValue);
            }

            return validValueCollection;
        }

        /// <summary>
        /// Extension to support simple addition of a range of values.
        /// </summary>
        public void AddRange(IEnumerable<ValidValue> items)
        {
            if(items == null) {
                throw new ArgumentNullException("items");
            }
            foreach(var item in items) {
                Add(item);
            }
        }
    }
}
