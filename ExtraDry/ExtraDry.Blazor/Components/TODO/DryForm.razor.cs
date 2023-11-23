﻿namespace ExtraDry.Blazor;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "DRY1500:Extra DRY Blazor components should have an interface.",
    Justification = "Decide fate of component")]
public partial class DryForm<T> : ComponentBase {
        
    [Parameter]
    public object? ViewModel { get; set; }

    [Parameter]
    public T? Model { get; set; }

    [Parameter]
    public EditMode EditMode { get; set; } = EditMode.Update;

    /// <summary>
    /// Represents the number of groups that are rendered in the first collection of fieldsets.
    /// The remainder are rendered in a second collection of fieldsets.
    /// CSS styles can render these two separately, i.e. making the second set scrollable.
    /// </summary>
    [Parameter]
    public int FixedGroups { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Inject]
    private ILogger<DryForm<T>> Logger { get; set; } = null!;

    private string? error;

    protected override void OnParametersSet()
    {
        if(ViewModel == null) {
            Logger.LogConsoleError("DryForm requires a ViewModel");
            return;
        }
        Description ??= new ViewModelDescription(typeof(T), ViewModel);
        if(Model != null) {
            FormDescription = new FormDescription(Description, Model);
        }
    }

    internal string ModelNameWebId => Slug.ToSlug(FormDescription?.ViewModelDescription?.ModelDisplayName ?? "");

    internal ViewModelDescription? Description { get; set; }

    internal FormDescription? FormDescription { get; set; }

    private async Task ExecuteAsync(CommandInfo command)
    {
        try {
            await command.ExecuteAsync(Model);
        }
        catch(DryException ex) {
            error = ex.ProblemDetails.Title;
        }
        catch(Exception) {
            error = "Unrecognized error occurred";
        }
    }


}