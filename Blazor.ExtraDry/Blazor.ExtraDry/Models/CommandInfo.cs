#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Models {

    /// <summary>
    /// Represents a command which wraps a method call and additional information about how to present
    /// the command using the method's signature and, optionally, the DisplayAttribute on the method.
    /// </summary>
    public class CommandInfo {

        /// <summary>
        /// Create a `CommandInfo` with a reference to the ViewModel it will execute on and the method to call.
        /// </summary>
        public CommandInfo(object viewModel, MethodInfo method)
        {
            ViewModel = viewModel;
            Method = method;
            Initialize(method);
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
        /// The type which determines how this command relates to other commands.
        /// This semantic information is used to determine UI layout.
        /// </summary>
        public CommandContext Context { get; set; }

        /// <summary>
        /// The reflected method for the command.
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// The caption of the command, taken from the `CommandAttribute` if available.
        /// If not, this is inferred from the signature of the `Method` by convention.
        /// </summary>
        public string? Caption { get; set; }

        /// <summary>
        /// The optional name of the icon to be displayed on buttons.
        /// This is just the stem of the name (e.g. 'plus') which is mixed with the theme to create a final name (e.g. 'fas fa-plus').
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// The view model that this command is defined as being part of.
        /// Used by `ExecuteAsync` to invoke the command on the correct object instance.
        /// </summary>
        public object ViewModel { get; set; }

        /// <summary>
        /// The type of arguments that this command works with, used to determine if and how many items can be selected.
        /// </summary>
        public CommandArguments Arguments { get; set; }

        /// <summary>
        /// A CSS class that is added to elements that can trigger the command.
        /// This has no intrinsic meaning but can be used by app to change appearance.
        /// </summary>
        public string DisplayClass => Context.ToString().ToLowerInvariant();

        /// <summary>
        /// Executes the underlying method with the provided arguments, ensuring that the proper number of arguments are provided.
        /// </summary>
        public async Task ExecuteAsync(object? arg = null) {
            if(Arguments == CommandArguments.None && arg != null) {
                throw new InvalidOperationException("Command does not take any arguments but one was supplied");
            }
            object?[]? args = Arguments switch {
                CommandArguments.Single => new object?[] { arg },
                CommandArguments.Multiple => new object?[] { GetStrongTypedSubset(arg) },
                _ => null,
            };
            var result = Method.Invoke(ViewModel, args);
            if(result is Task task) {
                await task;
            }
        }

        /// <summary>
        /// Helper for constructors.
        /// </summary>
        private void Initialize(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<CommandAttribute>();
            Caption = attribute?.Name ?? DefaultName(method.Name);
            Arguments = GetArgumentsType(method);
            if(attribute != null) {
                Icon = attribute.Icon;
                Context = attribute.Context;
            }
        }

        /// <summary>
        /// Determines a default name for the command based on a formatted version of the method name.
        /// </summary>
        private static string DefaultName(string name)
        {
            name = name.Replace("Async", "");
            name = Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled).Trim();
            return name;
        }

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

        private IList GetStrongTypedSubset(object? arg)
        {
            if(!(arg is IEnumerable)) {
                throw new ArgumentException("Parameter, while an object, must be of assignedable to type IEnumerable", nameof(arg));
            }
            var type = Method.GetParameters()[0].ParameterType.GenericTypeArguments[0];
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(type);
            var typedCollection = Activator.CreateInstance(constructedListType);
            if(typedCollection == null) {
                throw new InvalidOperationException($"Could not create type List<{type}> for CommandInfo");
            }
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
}
