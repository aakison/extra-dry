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
