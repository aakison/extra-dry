using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Blazor;

public class DecoratorInfo
{
    public DecoratorInfo(object decorator)
    {
        Decorator = decorator;
        GetReflectedDecoratorCommands(decorator);
        GetReflectedModel(decorator.GetType());
        SetListSelectMode();
    }

    public DecoratorInfo(Type modelType, object decorator)
    {
        ModelType = modelType;
        Decorator = decorator;
        GetReflectedDecoratorHyperLinks(decorator, modelType);
        GetReflectedModelProperties(modelType);
        GetReflectedDecoratorCommands(decorator);
        GetReflectedModel(modelType);
        SetListSelectMode();
    }

    public object Decorator { get; }

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
        var properties = modelType.GetProperties().ToList();
        var descriptions = properties.Select(e => new PropertyDescription(e)).ToList();
        var ordered = descriptions.OrderBy(e => e.Order ?? 10_000 + descriptions.IndexOf(e));
        foreach(var property in ordered) {
            var display = property.Display;
            if(display?.GetAutoGenerateField() ?? true) {
                FormProperties.Add(property);
            }
            if(!string.IsNullOrEmpty(display?.ShortName)) {
                TableProperties.Add(property);
            }
            if(property.Filter != null) {
                FilterProperties.Add(property);
            }
            if(UuidProperty == null && property.PropertyType == typeof(Guid)) {
                UuidProperty = property;
            }
        }
    }

    private void GetReflectedDecoratorCommands(object decorator)
    {
        if(decorator == null) {
            return;
        }
        var decoratorType = decorator.GetType();
        var methods = decoratorType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var commands = methods.Where(e => e.GetParameters().Length < 2 && e.GetCustomAttribute<CommandAttribute>() != null);
        var infos = commands
            .Select(e => new CommandInfo(decorator, e))
            .OrderBy(e => e.Context);
        foreach(var info in infos) {
            Commands.Add(info);
        }
    }

    private void GetReflectedDecoratorHyperLinks(object decorator, Type modelType)
    {
        if(decorator == null) {
            return;
        }
        var decoratorType = decorator.GetType();
        var methods = decoratorType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        var hyperlinks = methods.Where(e => e.GetParameters().Length < 2 && e.GetCustomAttribute<HyperlinkAttribute>() != null);
        var infos = hyperlinks.Select(e => new HyperlinkInfo(decorator, modelType, e));
        foreach(var info in infos) {
            HyperLinks.Add(info);
        }
    }

    [SuppressMessage("Style", "IDE0045:Convert to conditional expression", Justification = "False Positive")]
    private void SetListSelectMode()
    {
        if(Commands.Any(e => e.Arguments == CommandArguments.Multiple)) {
            ListSelectMode = ListSelectMode.Multiple;
        }
        else if(Commands.Where(e => e.Arguments == CommandArguments.Single
                && e.Context == CommandContext.Primary).Count() == 1) {
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
