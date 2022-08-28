namespace ExtraDry.Core;

public class ViewModel<T> : ISubjectViewModel
    where T : notnull 
{

    public ViewModel(T model)
    {
        Model = model;
    }

    public virtual string Code => string.Empty;

    public virtual string Title => Model.ToString() ?? string.Empty;

    public virtual string Subtitle => string.Empty;

    public virtual string Caption => string.IsNullOrWhiteSpace(Code) ? Title : $"{Title} ({Code})";

    public virtual string Icon => string.Empty;

    public virtual string Description => string.Empty;

    public T Model { get; }
}

