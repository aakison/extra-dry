using ExtraDry.Core.Validation;

namespace ExtraDry.Blazor.Components;

public class ValidationScopeContext
{

    public List<ValidationInfo> Results { get; } = [];

    public async Task ClearAsync()
    {
        Results.Clear();
        await ComputeStatusAsync();
    }

    public async Task AddAsync(string memberName, ValidationStatus status, string message)
    {
        Results.Add(new ValidationInfo(memberName, status, message));
        await ComputeStatusAsync();
    }

    public async Task ReplaceAsync(string memberName, ValidationStatus status, string message)
    {
        Results.RemoveAll(e => e.MemberName == memberName);
        Results.Add(new ValidationInfo(memberName, status, message));
        await ComputeStatusAsync();
    }

    public async Task RemoveAsync(string memberName)
    {
        Results.RemoveAll(e => e.MemberName == memberName);
        await ComputeStatusAsync();
    }

    public ValidationStatus Status { get; private set; }

    public EventCallback<ValidationStatus> OnStatusChanged { get; set; }

    /// <summary>
    /// Registers a field's revalidation callback so that it can be forced to reveal any
    /// currently silent validation errors, e.g. when the user attempts to submit the form.
    /// </summary>
    public void RegisterField(Func<Task> forceValidateAsync)
    {
        forceValidateHandlers.Add(forceValidateAsync);
    }

    /// <summary>
    /// Unregisters a field's revalidation callback, e.g. when the field is disposed.
    /// </summary>
    public void UnregisterField(Func<Task> forceValidateAsync)
    {
        forceValidateHandlers.Remove(forceValidateAsync);
    }

    /// <summary>
    /// Forces every registered field to revalidate and reveal any errors, converting any
    /// currently `Silent` results into `Failed` results so they are displayed to the user. Used
    /// when a submit is attempted so that untouched but invalid fields (e.g. empty required
    /// fields) become visible without requiring the user to interact with them first.
    /// </summary>
    public async Task ForceValidationAsync()
    {
        foreach(var handler in forceValidateHandlers.ToList()) {
            await handler();
        }
    }

    private readonly List<Func<Task>> forceValidateHandlers = [];

    private async Task ComputeStatusAsync()
    {
        var oldStatus = Status;
        if(Results.Any(e => e.Status == ValidationStatus.Failed)) {
            Status = ValidationStatus.Failed;
        }
        else if(Results.Any(e => e.Status == ValidationStatus.Silent)) {
            Status = ValidationStatus.Silent;
        }
        else {
            Status = ValidationStatus.Passed;
        }
        if(oldStatus != Status) {
            Console.WriteLine($"Validation status changed from {oldStatus} to {Status}");
            await OnStatusChanged.InvokeAsync(Status);
        }
    }

}

