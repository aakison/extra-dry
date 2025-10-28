using System.Text.RegularExpressions;

namespace ExtraDry.Blazor.Models;

/// <summary>
/// Represents a command which wraps a method call and additional information about how to present
/// the command using the method's signature and, optionally, the DisplayAttribute on the method.
/// </summary>
public partial class CommandInfo
{
    /// <summary>
    /// Create a `CommandInfo` with a reference to the ViewModel it will execute on and the method
    /// to call.
    /// </summary>
    public CommandInfo(object viewModel, MethodInfo method)
    {
        ViewModel = viewModel;
        Method = method;
        Initialize(method);
    }

    public CommandInfo(object viewModel, string methodName)
    {
        ViewModel = viewModel;
        Method = viewModel.GetType().GetMethod(methodName)
            ?? throw new ArgumentException($"No method found named {methodName}");
        Initialize(Method);
    }

    /// <summary>
    /// Convenience constructor when the method is parameterless.
    /// </summary>
    public CommandInfo(object viewModel, Action action)
    {
        ViewModel = viewModel;
        Method = action.Method;
        Initialize(action.Method);
    }

    /// <summary>
    /// Convenience constructor when the method is async and parameterless.
    /// </summary>
    public CommandInfo(object viewModel, Func<Task> action)
    {
        ViewModel = viewModel;
        Method = action.Method;
        Initialize(action.Method);
    }

    /// <summary>
    /// The type which determines how this command relates to other commands. This semantic
    /// information is used to determine UI layout.
    /// </summary>
    public CommandContext Context { get; set; } = CommandContext.Regular;

    /// <inheritdoc cref="ButtonTheme" />
    public ButtonTheme Theme { get; set; } = ButtonTheme.Normal;

    /// <summary>
    /// The category for this command which is used for filters.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// The reflected method for the command.
    /// </summary>
    public MethodInfo Method { get; set; }

    /// <summary>
    /// The caption of the command, taken from the `CommandAttribute` if available. If not, this is
    /// inferred from the signature of the `Method` by convention.
    /// </summary>
    public string? Caption { get; set; }

    /// <summary>
    /// The title of the command, which is displayed as a tooltip when hovering over the button.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// A comma-separated list of roles required for the command to be displayed. 
    /// </summary>
    public string? Roles { get; set; }

    /// <summary>
    /// The optional key of the icon to be displayed on buttons.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// The optional key of an icon to be displayed on the right of the button indicating what the
    /// button will visually do, e.g. "chevron-down" to indicate the result is a drop-down
    /// mini-dialog.
    /// </summary>
    public string? Affordance { get; set; }

    /// <summary>
    /// The view model that this command is defined as being part of. Used by `ExecuteAsync` to
    /// invoke the command on the correct object instance.
    /// </summary>
    public object ViewModel { get; set; }

    /// <summary>
    /// The type of arguments that this command works with, used to determine if and how many items
    /// can be selected.
    /// </summary>
    public CommandArguments Arguments { get; set; }

    /// <summary>
    /// A CSS class that is added to the button. This has no intrinsic meaning but can be used to
    /// inform additional styling.
    /// </summary>
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// A CSS class that is added to elements that can trigger the command. This has no intrinsic
    /// meaning but can be used by app to change appearance.
    /// </summary>
    public string DisplayClass => DataConverter.JoinNonEmpty(" ", CssClass, DataConverter.DisplayEnum(Context).ToLowerInvariant());

    public Func<bool> IsVisible { get; set; } = () => true;

    /// <summary>
    /// Executes the underlying method with the provided arguments, ensuring that the proper number
    /// of arguments are provided.
    /// </summary>
    public async Task ExecuteAsync(object? arg = null)
    {
        object?[]? args = Arguments switch {
            CommandArguments.Single => [arg],
            CommandArguments.Multiple => [GetStrongTypedSubset(arg)],
            _ => null,
        };
        var result = Method.Invoke(ViewModel, args);
        if(result is Task task) {
            await task;
        }
    }

    /// <summary>
    /// Determines a default name for the command based on a formatted version of the method name.
    /// </summary>
    private static string DefaultName(string name)
    {
        name = name.Replace("Async", "");
        name = DefaultNameFormatter().Replace(name, " $1").Trim();
        //name = Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled).Trim();
        return name;
    }

    [GeneratedRegex(@"(?<=[a-z])([A-Z])", RegexOptions.Compiled)]
    private static partial Regex DefaultNameFormatter();

    private static CommandArguments GetArgumentsType(MethodInfo method)
    {
        var parameters = method.GetParameters();
        if(parameters.Length == 0) {
            return CommandArguments.None;
        }
        var parameterType = parameters.First().ParameterType;
        if(parameterType.IsAssignableTo(typeof(string))) {
            return CommandArguments.Single; // Special case, don't treate as IEnumerable<char>.  WARNING: Any others?
        }
        else if(parameterType.IsAssignableTo(typeof(IEnumerable))) {
            return CommandArguments.Multiple;
        }
        else {
            return CommandArguments.Single;
        }
    }

    /// <summary>
    /// Helper for constructors.
    /// </summary>
    private void Initialize(MethodInfo method)
    {
        var attribute = method.GetCustomAttribute<CommandAttribute>();
        CssClass = attribute?.CssClass ?? "";
        Caption = attribute?.Name ?? DefaultName(method.Name);
        Arguments = GetArgumentsType(method);
        if(attribute != null) {
            Icon = attribute.Icon;
            Affordance = attribute.Affordance;
            Context = attribute.Context;
            Theme = attribute.Theme;
            Category = attribute.Category;
            Title = attribute.Title;
            Roles = attribute.Roles;
        }
    }

    private IList GetStrongTypedSubset(object? arg)
    {
        if(arg is not IEnumerable) {
            throw new ArgumentException("Parameter, while an object, must be of assignable to type IEnumerable", nameof(arg));
        }

        var parameterType = Method.GetParameters()[0].ParameterType;
        var type = parameterType.GenericTypeArguments[0];
        var listType = typeof(List<>);
        var constructedListType = listType.MakeGenericType(type);
        var typedCollection = Activator.CreateInstance(constructedListType)
            ?? throw new InvalidOperationException($"Could not create type List<{type}> for CommandInfo");
        var collection = (IList)typedCollection;
        var enumerable = (IEnumerable)arg;
        foreach(object item in enumerable) {
            if(item.GetType().IsAssignableTo(type)) {
                collection.Add(item);
            }
        }
        return collection;
    }
}
