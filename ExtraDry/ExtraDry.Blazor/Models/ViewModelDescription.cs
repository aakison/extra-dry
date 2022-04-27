#nullable enable

using ExtraDry.Blazor.Models;
using ExtraDry.Core;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ExtraDry.Blazor;

public class ViewModelDescription {

    public ViewModelDescription(object viewModel)
    {
        ViewModel = viewModel;
        GetReflectedViewModelCommands(viewModel);
        GetReflectedViewModelNavigations(viewModel);
        SetListSelectMode();
    }

    public ViewModelDescription(Type modelType, object viewModel)
    {
        ModelType = modelType;
        ViewModel = viewModel;
        GetReflectedModelProperties(modelType);
        GetReflectedViewModelCommands(viewModel);
        GetReflectedViewModelNavigations(viewModel);
        SetListSelectMode();
    }

    public object ViewModel { get; }

    public Type? ModelType { get; }

    public Collection<PropertyDescription> FormProperties { get; } = new();

    public Collection<PropertyDescription> TableProperties { get; } = new();

    public Collection<PropertyDescription> FilterProperties { get; } = new();

    public ListSelectMode ListSelectMode { get; private set; } = ListSelectMode.None;

    public Collection<CommandInfo> Commands { get; } = new();

    public Collection<NavigationDescription> Navigations { get; } = new();

    public CommandInfo? SelectCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Primary && e.Arguments == CommandArguments.Single);

    public ReadOnlyCollection<CommandInfo> MenuCommands => new(Commands.Where(e => e.Arguments == CommandArguments.None).ToList());

    public ReadOnlyCollection<CommandInfo> ContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Single).ToList());

    public ReadOnlyCollection<CommandInfo> MultiContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Multiple).ToList());

    public bool HasNavigationGroups => Navigations.Any(e => !string.IsNullOrWhiteSpace(e.Group));

    public IEnumerable<string?> NavigationGroups => Navigations.Select(e => e.Group).Distinct();

    public IEnumerable<NavigationDescription> NavigationsInGroup(string group) => Navigations.Where(e => e.Group == group);

    public string ModelDisplayName { get; private set; } = string.Empty;

    private void GetReflectedModelProperties(Type modelType)
    {
        ModelDisplayName = modelType.GetCustomAttribute<DisplayAttribute>()?.Name ?? modelType.Name;
        var properties = modelType.GetProperties();
        foreach(var property in properties) {
            var display = property.GetCustomAttribute<DisplayAttribute>();
            var col = new PropertyDescription(property);
            if(display != null && (display.GetAutoGenerateField() ?? true)) {
                FormProperties.Add(col);
            }
            if(!string.IsNullOrEmpty(display?.ShortName)) {
                TableProperties.Add(col);
            }
            if(col.Filter != null) {
                FilterProperties.Add(col);
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

    private void GetReflectedViewModelNavigations(object viewModel)
    {
        if(viewModel == null) {
            return;
        }
        var viewModelType = viewModel.GetType();
        var members = viewModelType.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var navigationMembers = members.Where(e => e.GetCustomAttribute<NavigationAttribute>() != null);
        var unorderedNavigations = new List<NavigationDescription>();
        foreach(var member in navigationMembers) {
            if(member is PropertyInfo property) {
                unorderedNavigations.Add(new NavigationDescription(viewModel, property));
            }
            if(member is MethodInfo method) {
                unorderedNavigations.Add(new NavigationDescription(viewModel, method));
            }
        }
        foreach(var desc in unorderedNavigations.OrderBy(e => e.Order)) {
            Navigations.Add(desc);
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
