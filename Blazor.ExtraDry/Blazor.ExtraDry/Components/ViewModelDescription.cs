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
            GetReflectedViewModelProperties(viewModel);
            SetListSelectMode();
        }

        public ViewModelDescription(Type modelType, object viewModel)
        {
            ModelType = modelType;
            ViewModel = viewModel;
            GetReflectedModelProperties(modelType);
            GetReflectedViewModelProperties(viewModel);
            SetListSelectMode();
        }

        public object ViewModel { get; }

        public Type ModelType { get; }

        public Collection<PropertyDescription> FormProperties { get; } = new Collection<PropertyDescription>();

        public Collection<PropertyDescription> TableProperties { get; } = new Collection<PropertyDescription>();

        public ListSelectMode ListSelectMode { get; private set; } = ListSelectMode.None;

        public Collection<CommandInfo> Commands { get; } = new Collection<CommandInfo>();

        public CommandInfo SelectCommand => Commands.FirstOrDefault(e => e.Context == CommandContext.Primary && e.Arguments == CommandArguments.Single);

        public ReadOnlyCollection<CommandInfo> MenuCommands => new(Commands.Where(e => e.Arguments == CommandArguments.None).ToList());

        public ReadOnlyCollection<CommandInfo> ContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Single).ToList());

        public ReadOnlyCollection<CommandInfo> MultiContextCommands => new(Commands.Where(e => e.Arguments == CommandArguments.Multiple).ToList());

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

        private void GetReflectedViewModelProperties(object viewModel) { 
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
}
