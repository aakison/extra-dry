namespace ExtraDry.Blazor.Models;

/// <summary>
/// Represents a hyperlink which wraps a method call and additional information about how to present
/// the hyperlink using the method's signature.
/// </summary>
public partial class HyperlinkInfo
{
    /// <summary>
    /// Create a `HyperLinkInfo` with a reference to the ViewModel it will execute on and the method to call.
    /// </summary>
    public HyperlinkInfo(object viewModel, Type modelType, MethodInfo method)
    {
        ViewModel = viewModel;
        ModelType = modelType;
        Method = method;
        Initialize(method);
    }

    /// <summary>
    /// The reflected method for the command.
    /// </summary>
    public MethodInfo Method { get; set; }

    /// <summary>
    /// The view model that this hyperlink is defined as being part of.
    /// Used by `ExecuteAsync` to invoke the command on the correct object instance.
    /// </summary>
    public object ViewModel { get; set; }

    public Type ModelType { get; set; }

    /// <inheritdoc cref="HyperlinkAttribute.PropertyName" />
    public string? PropertyName{ get; set; }

    public HyperlinkContext Execute(object? arg = null)
    {
        return Method.Invoke(ViewModel, [arg]) as HyperlinkContext 
            ?? throw new InvalidOperationException("The hyperlink method is not correctly populated");
    }

    /// <summary>
    /// Helper for constructors.
    /// </summary>
    private void Initialize(MethodInfo method)
    {
        var attribute = method.GetCustomAttribute<HyperlinkAttribute>();
        if(attribute != null) {
            PropertyName = attribute.PropertyName;
            if(method.ReturnType != typeof(HyperlinkContext)) {
                throw new InvalidOperationException("The return type of the method with the Hyperlink attribute must return a HyperlinkContext");
            }

            var parameters = method.GetParameters();
            if(parameters.Length != 1 || parameters.First().ParameterType != ModelType) {
                throw new InvalidOperationException($"The method with the Hyperlink attribute must only take a parameter of type {ModelType}");
            }
        }
    }
}
