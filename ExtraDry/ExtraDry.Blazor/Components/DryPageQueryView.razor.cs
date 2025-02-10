using ExtraDry.Blazor.Components.Internal;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor;

[SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.", Justification = "Used for cascading data, not visual component.")]
public partial class DryPageQueryView : ComponentBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// A page query that encloses this filter and other components. All filter aware components
    /// will attach to this query builder. When filters change the components will use this query
    /// builder to notify other components that an update is required, e.g. for a table component
    /// to refresh its contents based on the new filter. Filters will also refresh themselves to
    /// changes using a two-way binding.
    /// </summary>
    [CascadingParameter]
    public QueryBuilder PageQueryBuilder { get; set; } = new();
}
