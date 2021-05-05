using Blazor.ExtraDry.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Blazor.ExtraDry {
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

        public Type ModelType { get; }

        public Collection<PropertyDescription> FormProperties { get; } = new Collection<PropertyDescription>();

        public Collection<PropertyDescription> TableProperties { get; } = new Collection<PropertyDescription>();

        public ListSelectMode ListSelectMode { get; private set; } = ListSelectMode.None;

        public Collection<CommandInfo> Commands { get; } = new Collection<CommandInfo>();

        public List<NavigationDescription> Navigations { get; } = new();

        public CommandInfo SelectCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Primary && e.Arguments == CommandArguments.Single);

        public ReadOnlyCollection<CommandInfo> MenuCommands => new(Commands.Where(e => e.Arguments == CommandArguments.None).ToList());

        public ReadOnlyCollection<CommandInfo> ContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Single).ToList());

        public ReadOnlyCollection<CommandInfo> MultiContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Multiple).ToList());

        public bool HasNavigationGroups => Navigations.Any(e => !string.IsNullOrWhiteSpace(e.Group));

        public IEnumerable<string> NavigationGroups => Navigations.Select(e => e.GroupName).Distinct();

        public IEnumerable<NavigationDescription> NavigationsInGroup(string group) => Navigations.Where(e => e.Group == group);

        private void GetReflectedModelProperties(Type modelType)
        {
            if(modelType == null) {
                return;
            }
            var properties = modelType.GetProperties();
            foreach(var property in properties) {
                var display = property.GetCustomAttribute<DisplayAttribute>();
                var col = new PropertyDescription(property);
                if(!string.IsNullOrEmpty(display?.Name)) {
                    FormProperties.Add(col);
                }
                if(!string.IsNullOrEmpty(display?.ShortName)) {
                    TableProperties.Add(col);
                }
            }
        }

        private void GetReflectedViewModelCommands(object viewModel) { 
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
}
