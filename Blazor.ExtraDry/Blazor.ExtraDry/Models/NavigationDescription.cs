#nullable enable

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blazor.ExtraDry.Models {

    /// <summary>
    /// Represents a property that contains a link or a method that performs a navigation.
    /// </summary>
    public class NavigationDescription {

        /// <summary>
        /// Create a `NavigationDescription` with a reference to the ViewModel it will execute on and the method to call.
        /// </summary>
        public NavigationDescription(object viewModel, MethodInfo method)
        {
            ViewModel = viewModel;
            Method = method;
            Navigation = Method.GetCustomAttribute<NavigationAttribute>();
            Initialize();
        }

        /// <summary>
        /// Create a `NavigationDescription` which uses a property to determine the href.
        /// </summary>
        public NavigationDescription(object viewModel, PropertyInfo property)
        {
            ViewModel = viewModel;
            Property = property;
            Navigation = Property.GetCustomAttribute<NavigationAttribute>();
            Initialize();
        }

        /// <summary>
        /// The reflected property for the navigation href.
        /// Mutually exlusive with `Method`.
        /// </summary>
        public PropertyInfo? Property { get; private set; }

        /// <summary>
        /// The reflected method for the navigation link to execute.
        /// Mutually exlusive with `Property`.
        /// </summary>
        public MethodInfo? Method { get; private set; }

        /// <summary>
        /// The navigation attribute that defines the property/method as being for navigation.
        /// </summary>
        public NavigationAttribute? Navigation { get; private set; }

        public string? Group { get; set; }

        /// <summary>
        /// The caption of the command, taken from the `CommandAttribute` if available.
        /// If not, this is inferred from the signature of the `Method` by convention.
        /// </summary>
        public string? Caption { get; set; }

        /// <summary>
        /// The optional name of the icon to be displayed on links.
        /// This is just the stem of the name (e.g. 'plus') which is mixed with the theme to create a final name (e.g. 'fas fa-plus').
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// The optional name of an image to be displayed with the link.
        /// </summary>
        public string? Image { get; set; }


        /// <summary>
        /// The optional order of the navigation item, if omitted then the file order is respected.
        /// Note, however, method based navigations will list before property based navigations.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// The view model that this command is defined as being part of.
        /// Used by `ExecuteAsync` to invoke the command on the correct object instance.
        /// </summary>
        public object ViewModel { get; set; }


        public string? ActiveMatch { get; set; }

        /// <summary>
        /// Indicates if the navigation uses an Href instead of an onclick...
        /// </summary>
        public bool HasHref => Property != null;

        /// <summary>
        /// The href as defined by the `Property` or non-functional placeholder otherwise if command based navigation.
        /// </summary>
        public string Href => Property?.GetValue(ViewModel)?.ToString() ?? "javascript:void(0)";

        public bool HasImage => !string.IsNullOrWhiteSpace(Image);

        public bool HasIcon => !string.IsNullOrWhiteSpace(Icon);

        public bool UriMatch(NavigationManager navigation)
        {
            var relativeUri = navigation.Uri.Remove(0, navigation.BaseUri.Length);
            var match = ActiveMatch == null ? Href.TrimStart('/') : ActiveMatch.TrimStart('/');
            var isMatch = string.IsNullOrWhiteSpace(match) ? 
                string.IsNullOrWhiteSpace(relativeUri) :
                relativeUri?.Contains(match) ?? false;
            return isMatch;
        }

        /// <summary>
        /// Executes the underlying method with the provided arguments, ensuring that the proper number of arguments are provided.
        /// </summary>
        public async Task ExecuteAsync()
        {
            if(Method != null) {
                var result = Method.Invoke(ViewModel, null);
                if(result is Task task) {
                    await task;
                }
            }
        }

        private void Initialize()
        {
            Caption = Navigation?.Caption;
            if(string.IsNullOrWhiteSpace(Caption)) {
                Caption = DefaultName(Method?.Name ?? Property?.Name ?? "");
            }
            Icon = Navigation?.Icon;
            Image = Navigation?.Image;
            Group = Navigation?.Group;
            Order = Navigation?.Order ?? 0;
            ActiveMatch = Navigation?.ActiveMatch;
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


    }
}
