namespace ExtraDry.Blazor;

/// <summary>
/// Helper to consistently resolve the display fields for a Model given an optional View Model and/or interface.
/// </summary>
internal static class ViewModelHelper {

    public static string? Code<TModel>(TModel model, ISubjectViewModel<TModel>? viewModel, string? _default = null)
    {
        if(viewModel != null) {
            return viewModel.Code(model);
        }
        else if(model is ISubjectViewModel subject) {
            return subject.Code;
        }
        else {
            return _default;
        }
    }

    public static string? Title<TModel>(TModel model, ISubjectViewModel<TModel>? viewModel, string? _default = null)
    {
        if(viewModel != null) {
            return viewModel.Title(model);
        }
        else if(model is ISubjectViewModel subject) {
            return subject.Title;
        }
        else {
            return _default;
        }
    }

    public static string? Subtitle<TModel>(TModel model, ISubjectViewModel<TModel>? viewModel, string? _default = null)
    {
        if(viewModel != null) {
            return viewModel.Subtitle(model);
        }
        else if(model is ISubjectViewModel subject) {
            return subject.Subtitle;
        }
        else {
            return _default;
        }
    }

    public static string? Caption<TModel>(TModel model, ISubjectViewModel<TModel>? viewModel, string? _default = null)
    {
        if(viewModel != null) {
            return viewModel.Caption(model);
        }
        else if(model is ISubjectViewModel subject) {
            return subject.Caption;
        }
        else {
            return _default;
        }
    }

    public static string? Thumbnail<TModel>(TModel model, ISubjectViewModel<TModel>? viewModel, string? _default = null)
    {
        if(viewModel != null) {
            return viewModel.Icon(model);
        }
        else if(model is ISubjectViewModel subject) {
            return subject.Icon;
        }
        else {
            return _default;
        }
    }

    public static string? Description<TModel>(TModel model, ISubjectViewModel<TModel>? viewModel, string? _default = null)
    {
        if(viewModel != null) {
            return viewModel.Description(model);
        }
        else if(model is ISubjectViewModel subject) {
            return subject.Description;
        }
        else {
            return _default;
        }
    }

}
