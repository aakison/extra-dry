namespace ExtraDry.Blazor;

public interface IDryInput<T> : IExtraDryComponent
{
    public T? Model { get; set; }
    PropertyDescription? Property { get; set; }
    EventCallback<ChangeEventArgs>? OnChange { get; set; }
    EditMode EditMode { get; set; }
}
