using System.Collections.ObjectModel;

namespace ExtraDry.Core;

/// <summary>
/// A collection of Table that supports List and Linq operations.
/// </summary>
/// <remarks>
/// This is a shallow wrapper to provide some future-proofing.
/// </remarks>
public class TableColumnCollection : Collection<TableColumn> {
}
