using System.Reflection.Metadata;

namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a freshness indicator field. Prefer the use of <see cref="DryInput{T}" /> instead
/// of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputFreshnessIndicator<T> : DryInputBase<T>
{
    
    /// <inheritdoc cref="DryInput{T}.ReadOnly" />
    [Parameter]
    public bool ReadOnly { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if(Model == null || Property == null) {
            return;
        }

        var property = Property?.GetValue(Model) ?? new UserTimestamp();

        var userGuid = ((UserTimestamp)property).User;
        var timeStamp = ((UserTimestamp)property).Timestamp;

        var user = "";
        Value = $"updated {DataConverter.DateToRelativeTime(timeStamp)}";
        
        if(DisplayNameProvider != null) {
            user = await DisplayNameProvider.ResolveDisplayNameAsync(userGuid);
        }
        else {
            user = userGuid;
        }

        Value = $"{user} updated {DataConverter.DateToRelativeTime(timeStamp)}";
    }

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", ReadOnlyCss, CssClass);

    private string? Value { get; set; } = "";

    [Inject]
    private IDisplayNameProvider? DisplayNameProvider { get; set; }
}
