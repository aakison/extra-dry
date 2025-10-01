using ExtraDry.Blazor;
using ExtraDry.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace ExtraDry.Blazor.Components.Standard;

public partial class InputDateRangeField : ComponentBase, IInputField<string>
{
    /// <summary>
    /// The display text for the four options for the date range. These are optional and if not
    /// provided will be the values:
    /// - On
    /// - Off
    /// - Expires
    /// - Range
    /// </summary>
    [Parameter]
    public IDictionary<string, string> Data { get; set; } = new Dictionary<string, string>();

    /// <summary>
    /// The date range value in the format "yyyy-MM-dd,yyyy-MM-dd"
    /// </summary>
    [Parameter]
    public string Value { get; set; } = "";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public bool ReadOnly { get; set; }

    [Parameter]
    public string CssClass { get; set; } = "";

    [Parameter]
    public string Icon { get; set; } = "";

    [Parameter]
    public string Affordance { get; set; } = "";

    [Parameter]
    public string Placeholder { get; set; } = "";

    [Parameter]
    public string Id { get; set; } = "";

    public string ValidationMessage { get; set; } = "";

    protected override void OnInitialized()
    {
        InputId = Id switch {
            "" => $"inputDateField{++instanceCount}",
            _ => Id,
        };
    }

    protected override void OnParametersSet()
    {
        if(SelectData.Count == 0) {
            var displayValues = Data.Values.ToList();
            SelectData[OnKey] = Data.Count > 0 ? displayValues[0] : "On";
            SelectData[OffKey] = Data.Count > 1 ? displayValues[1] : "Off";
            SelectData[ExpiresKey] = Data.Count > 2 ? displayValues[2] : "Expires";
            SelectData[RangeKey] = Data.Count > 3 ? displayValues[3] : "Range";
        }
        if(!string.IsNullOrWhiteSpace(Value) && SelectValue == "") {
            SelectValue = OnKey;
            var parts = Value.Split('/', 2);
            if(parts.Length == 2) {
                if(DateTime.TryParse(parts[0], out var from) && DateTime.TryParse(parts[1], out var to)) {
                    Console.WriteLine($"Parsed from {from} to {to}");
                    FromDate = from == DateTime.MinValue.Date ? "" : from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    ToDate = to == DateTime.MaxValue.Date || to == DateTime.MinValue.Date ? "" : to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    Console.WriteLine($"FromDate is {FromDate}, ToDate is {ToDate}");
                    if(from == DateTime.MinValue.Date && to == DateTime.MaxValue.Date) {
                        Console.WriteLine($"setting on");
                        SelectValue = OnKey;
                    }
                    else if(from == DateTime.MinValue.Date && to == DateTime.MinValue.Date) {
                        Console.WriteLine($"setting off");
                        SelectValue = OffKey;
                    }
                    else if(from == DateTime.MinValue.Date) {
                        Console.WriteLine($"setting expires");
                        SelectValue = ExpiresKey;
                    }
                    else {
                        Console.WriteLine($"setting range");
                        SelectValue = RangeKey;
                    }
                }
                Console.WriteLine($"SelectValue derived from {SelectValue}");
            }
            EnableFields();
        }
    }

    private Dictionary<string, string> SelectData { get; set; } = [];

    private string SelectValue { get; set; } = "";

    private string FromDate { get; set; } = "";

    private string ToDate { get; set; } = "";

    private bool FromReadOnly { get; set; }

    private bool ToReadOnly { get; set; }

    private bool IsValid { get; set; } = true;

    private bool DisplayIcon => Icon != "";

    private bool DisplayAffordance => Affordance != "" && !ReadOnly;

    private string ReadOnlyCss => ReadOnly ? "readonly" : string.Empty;

    private string CssClasses => DataConverter.JoinNonEmpty(" ", "input", "date", ReadOnlyCss, IsValidCss, CssClass);

    private string InputId { get; set; } = "";

    private string FromInputId => $"{InputId}_from";

    private string ToInputId => $"{InputId}_to";

    private string IsValidCss => IsValid ? "valid" : "invalid";

    private async Task OnSelectValueSet(string? value)
    {
        if(string.IsNullOrWhiteSpace(value)) {
            return;
        }
        SelectValue = value;
        CalculateValue();
        await ValueChanged.InvokeAsync(Value);
        EnableFields();
        Validate();
    }

    private async Task OnFromDateChanged(ChangeEventArgs e)
    {
        var value = e.Value?.ToString() ?? "";
        FromDate = value;
        CalculateValue();
        await ValueChanged.InvokeAsync(Value);
        Validate();
    }

    private async Task OnToDateChanged(ChangeEventArgs e)
    {
        var value = e.Value?.ToString() ?? "";
        ToDate = value;
        CalculateValue();
        await ValueChanged.InvokeAsync(Value);
        Validate();
    }

    private void EnableFields()
    {
        FromReadOnly = SelectValue switch {
            OnKey => true,
            OffKey => true,
            ExpiresKey => true,
            RangeKey => false,
            _ => true,
        };
        ToReadOnly = SelectValue switch {
            OnKey => true,
            OffKey => true,
            ExpiresKey => false,
            RangeKey => false,
            _ => true,
        };
    }

    private void CalculateValue()
    {
        Value = SelectValue switch {
            OnKey => $"{MinDate()}/{MaxDate()}",
            OffKey => $"{MinDate()}/{MinDate()}",
            ExpiresKey => $"{MinDate()}/{ResolvedToDate()}",
            _ => $"{ResolvedFromDate()}/{ResolvedToDate()}",
        };
        Console.WriteLine($"Calculated Value: {Value}");

        string ResolvedFromDate() => string.IsNullOrWhiteSpace(FromDate) ? FormatDate(DateTime.Now) : FromDate;
        string ResolvedToDate() => string.IsNullOrWhiteSpace(ToDate) ? FormatDate(DateTime.Now) : ToDate;
        string MinDate() => FormatDate(DateTime.MinValue);
        string MaxDate() => FormatDate(DateTime.MaxValue);
        string FormatDate(DateTime date) => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    private void Validate()
    {
        if(ValidationModel == null) {
            return;
        }
        var validator = new DataValidator();
        if(validator.ValidateProperties(ValidationModel, ValidationProperty)) {
            UpdateValidationUI(true, string.Empty);
        }
        else {
            UpdateValidationUI(false, string.Join("; ", validator.Errors.Select(e => e.ErrorMessage)));
        }
    }

    private void UpdateValidationUI(bool valid, string message)
    {
        IsValid = valid;
        ValidationMessage = DryValidationSummary.FormatMessage(ValidationProperty, message);
        StateHasChanged();
    }

    private const string OnKey = "on";

    private const string OffKey = "off";

    private const string ExpiresKey = "expires";

    private const string RangeKey = "range";

    #region For IInputField<string>

    /// <inheritdoc />
    [Parameter]
    public PropertySize Size { get; set; } = PropertySize.Medium;

    /// <inheritdoc />
    [Parameter]
    public string Description { get; set; } = "";

    /// <inheritdoc />
    [Parameter]
    public bool ShowDescription { get; set; } = false;

    /// <inheritdoc />
    [Parameter]
    public string Caption { get; set; } = "";

    [Parameter]
    public string FromCaption { get; set; } = "Start Date";

    [Parameter]
    public string ToCaption { get; set; } = "End Date";

    /// <inheritdoc />
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    #endregion For IInputField<string>

    #region For IValidatableField ??? To create if good idea...

    [Parameter]
    public object? ValidationModel { get; set; }

    [Parameter]
    public string ValidationProperty { get; set; } = "";

    #endregion For IValidatableField ??? To create if good idea...

    private static int instanceCount;
}
