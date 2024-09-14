namespace ExtraDry.Blazor.Components.Internal;

internal class ItemComparer<T>(
    PropertyDescription property, 
    bool ascending) 
    : IComparer<ListItemInfo<T>> 
{
    public PropertyDescription Property { get; } = property;

    public bool Ascending { get; } = ascending;

    private readonly int scale = ascending ? 1 : -1;

    private readonly bool propertyIsEnum = property.HasDiscreteValues;

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
        else if(x == null || x.Item == null) {
            return 1;
        }
        else if(y == null || y.Item == null) {
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
