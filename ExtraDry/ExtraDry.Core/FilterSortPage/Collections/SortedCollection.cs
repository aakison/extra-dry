using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Core;

/// <summary>
/// A sorted and filtered collection of items from the API.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "Not a C# collection, but an over-the-wire collection.")]
public class SortedCollection<T> : FilteredCollection<T>
{


}
