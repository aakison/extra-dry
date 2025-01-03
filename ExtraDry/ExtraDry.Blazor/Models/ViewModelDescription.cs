using System.Collections.ObjectModel;

namespace ExtraDry.Blazor;

public class ViewModelDescription
{
    public ViewModelDescription(object viewModel)
    {
        ViewModel = viewModel;
        GetReflectedViewModelCommands(viewModel);
        GetReflectedModel(viewModel.GetType());
        SetListSelectMode();
    }

    public ViewModelDescription(Type modelType, object viewModel)
    {
        ModelType = modelType;
        ViewModel = viewModel;
        GetReflectedViewModelHyperLinks(viewModel, modelType);
        GetReflectedModelProperties(modelType);
        GetReflectedViewModelCommands(viewModel);
        GetReflectedModel(modelType);
        SetListSelectMode();
    }

    public object ViewModel { get; }

    public Type? ModelType { get; }

    public Collection<PropertyDescription> FormProperties { get; } = [];

    public Collection<PropertyDescription> TableProperties { get; } = [];

    public Collection<PropertyDescription> FilterProperties { get; } = [];

    public PropertyDescription? UuidProperty { get; private set; }

    public ListSelectMode ListSelectMode { get; private set; } = ListSelectMode.None;

    public Collection<CommandInfo> Commands { get; } = [];

    public Collection<HyperlinkInfo> HyperLinks { get; } = [];

    public CommandInfo? SelectCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Primary && e.Arguments == CommandArguments.Single);

    public CommandInfo? DefaultCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Default && e.Arguments == CommandArguments.Single);

    public HyperlinkInfo? HyperLinkFor(string propertyName) => HyperLinks.FirstOrDefault(e => e.PropertyName == propertyName);

    public ReadOnlyCollection<CommandInfo> MenuCommands => new(Commands.Where(e => e.Arguments == CommandArguments.None).ToList());

    public ReadOnlyCollection<CommandInfo> ContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Single).ToList());

    public ReadOnlyCollection<CommandInfo> MultiContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Multiple).ToList());

    public string ModelDisplayName { get; private set; } = string.Empty;

    public FormatAttribute? Format { get; private set; }

    public string Icon { get; private set; } = string.Empty;

    private void GetReflectedModel(Type modelType)
    {
        Format = modelType.GetCustomAttribute<FormatAttribute>();
        Icon = Format?.Icon ?? string.Empty;
    }

    private void GetReflectedModelProperties(Type modelType)
    {
        ModelDisplayName = modelType.GetCustomAttribute<DisplayAttribute>()?.Name ?? modelType.Name;
        var properties = modelType.GetProperties();
        foreach(var property in properties) {
            var display = property.GetCustomAttribute<DisplayAttribute>();
            var col = new PropertyDescription(property);
            if(display?.GetAutoGenerateField() ?? true) {
                FormProperties.Add(col);
            }
            if(!string.IsNullOrEmpty(display?.ShortName)) {
                TableProperties.Add(col);
            }
            if(col.Filter != null) {
                FilterProperties.Add(col);
            }
            if(UuidProperty == null && property.PropertyType == typeof(Guid)) {
                UuidProperty = col;
            }
        }
    }

    private void GetReflectedViewModelCommands(object viewModel)
    {
        if(viewModel == null) {
            return;
        }
        var viewModelType = viewModel.GetType();
        var methods = viewModelType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var commands = methods.Where(e => e.GetParameters().Length < 2 && e.GetCustomAttribute<CommandAttribute>() != null);
        var infos = commands
            .Select(e => new CommandInfo(viewModel, e))
            .OrderBy(e => e.Context);
        foreach(var info in infos) {
            Commands.Add(info);
        }
    }

    private void GetReflectedViewModelHyperLinks(object viewModel, Type modelType)
    {
        if(viewModel == null) {
            return;
        }
        var viewModelType = viewModel.GetType();
        var methods = viewModelType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var hyperlinks = methods.Where(e => e.GetParameters().Length < 2 && e.GetCustomAttribute<HyperlinkAttribute>() != null);
        var infos = hyperlinks.Select(e => new HyperlinkInfo(viewModel, modelType, e));
        foreach(var info in infos) {
            HyperLinks.Add(info);
        }
    }

    private void SetListSelectMode()
    {
        if(Commands.Any(e => e.Arguments == CommandArguments.Multiple)) {
            ListSelectMode = ListSelectMode.Multiple;
        }
        else if(Commands.Where(e => e.Arguments == CommandArguments.Single && e.Context == CommandContext.Primary).Count() == 1) {
            ListSelectMode = ListSelectMode.Action;
        }
        else if(ContextCommands.Count != 0) {
            ListSelectMode = ListSelectMode.Single;
        }
        else {
            ListSelectMode = ListSelectMode.None;
        }
    }
}
