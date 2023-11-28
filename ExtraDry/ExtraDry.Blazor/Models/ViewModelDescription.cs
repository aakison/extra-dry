using System.Collections.ObjectModel;

namespace ExtraDry.Blazor;

public class ViewModelDescription {

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
        GetReflectedModelProperties(modelType);
        GetReflectedViewModelCommands(viewModel);
        GetReflectedModel(modelType);
        SetListSelectMode();
    }

    public object ViewModel { get; }

    public Type? ModelType { get; }

    public Collection<PropertyDescription> FormProperties { get; } = new();

    public Collection<PropertyDescription> TableProperties { get; } = new();

    public Collection<PropertyDescription> FilterProperties { get; } = new();

    public PropertyDescription? UuidProperty { get; private set; }

    public ListSelectMode ListSelectMode { get; private set; } = ListSelectMode.None;

    public Collection<CommandInfo> Commands { get; } = new();

    public CommandInfo? SelectCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Primary && e.Arguments == CommandArguments.Single);

    public CommandInfo? DefaultCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Default && e.Arguments == CommandArguments.Single);

    public CommandInfo? DefaultCommandFor(string propertyName) => Commands.FirstOrDefault(e => e.PropertyName == propertyName);

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
            var formProperty = col.HasTextRepresentation;
            // If no display attribute, then infer from rules
            if(col.Rules != null && display == null
                && (col.Rules.CreateAction == RuleAction.Block || col.Rules.CreateAction == RuleAction.Ignore)
                && (col.Rules.UpdateAction == RuleAction.Block || col.Rules.UpdateAction == RuleAction.Ignore)) {
                formProperty = false;
            }
            // If display attribute, assume display unless explicitly set to false
            if(display != null) {
                formProperty = display.GetAutoGenerateField() ?? true;
            }
            if(formProperty) {
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

    private void SetListSelectMode()
    {
        if(Commands.Any(e => e.Arguments == CommandArguments.Multiple)) {
            ListSelectMode = ListSelectMode.Multiple;
        }
        else if(Commands.Where(e => e.Arguments == CommandArguments.Single && e.Context == CommandContext.Primary).Count() == 1) {
            ListSelectMode = ListSelectMode.Action;
        }
        else if(ContextCommands.Any()) {
            ListSelectMode = ListSelectMode.Single;
        }
        else {
            ListSelectMode = ListSelectMode.None;
        }
    }

}
