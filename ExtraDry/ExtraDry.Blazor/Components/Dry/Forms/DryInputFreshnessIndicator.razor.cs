namespace ExtraDry.Blazor.Forms;

/// <summary>
/// A DRY wrapper around a freshness indicator field. Prefer the use of <see cref="DryInput{T}" />
/// instead of this component as it is more flexible and supports more data types.
/// </summary>
public partial class DryInputFreshnessIndicator<T>
    : DryInputBase<T>
    where T : class
{
    /// <inheritdoc cref="DryInput{T}.ReadOnly" />
    [Parameter]
    public bool ReadOnly { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        var property = Property?.GetValue(Model) ?? new UserTimestamp();

        var timestamp = ((UserTimestamp)property).Timestamp;
        var userId = ((UserTimestamp)property).User;

        if(timestamp == DateTime.MinValue) {
            Value = "not saved yet";
            return;
        }

        var relativeTime = DataConverter.DateToRelativeTime(timestamp);
        var user = await ResolveUserDisplayName(userId);
        Value = $"{relativeTime} by {user}";
    }

    private async Task<string> ResolveUserDisplayName(string userId)
    {
        if(string.IsNullOrWhiteSpace(userId)) {
            return "...";
        }
        if(Guid.TryParse(userId, out _)) {
            if(DisplayNameProvider != null) {
                return await DisplayNameProvider.ResolveDisplayNameAsync(userId);
            }
            return "...";
        }
        return userId;
    }

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "user-timestamp", "readonly", CssClass);

    private string? Value { get; set; } = "";

    [Inject]
    private IDisplayNameProvider? DisplayNameProvider { get; set; }
}
