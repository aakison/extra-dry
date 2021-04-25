#nullable enable

using Blazor.ExtraDry.Internal;
using Blazor.ExtraDry.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryForm<T> : ComponentBase {
        
        [Parameter]
        public object? ViewModel { get; set; }

        [Parameter]
        public T? Model { get; set; }

        [Parameter]
        public EditMode EditMode { get; set; } = EditMode.Update;

        [Inject]
        private ILogger<DryForm<T>>? Logger { get; set; }

        private string? error;

        private ViewModelDescription? description;

        protected override void OnParametersSet()
        {
            if(ViewModel == null) {
                Logger?.LogError("DryForm requires a ViewModel");
                return;
            }
            if(description == null) {
                description = new ViewModelDescription(typeof(T), ViewModel);
            }
            if(Model != null && !Fieldsets.Any()) {
                PopulateFieldsets(description.FormProperties, Model);
            }
        }

        private List<FormFieldset> Fieldsets { get; } = new List<FormFieldset>();

        private void PopulateFieldsets(Collection<DryProperty> properties, object model)
        {
            var currentFieldset = new FormFieldset();
            var currentLine = new FormLine(model);
            int currentSize = 0;
            for(int i = 0; i < properties.Count; ++i) {
                var property = properties[i];
                Console.WriteLine($"property: {property.Property.Name}, header: {property.Header?.Title}");
                if(property.Header != null) {
                    FlushCurrentFieldset();
                    currentFieldset.Legend = property.Header.Title;
                    currentFieldset.Column = property.Header.Column;
                }
                if(property.ChildModel != null) {
                    FlushCurrentLine();
                    // add header
                    var submodel = property.GetValue(model);
                    currentFieldset.Lines.Add(new FormLine(submodel));
                    // Add children
                    PopulateFieldsets(property.ChildModel.FormProperties, submodel);
                }
                else {
                    var size = PredictSize(property);
                    if(currentSize + size <= 4) {
                        Console.WriteLine($"  Adding to current line size {size}");
                        currentSize += size;
                    }
                    else {
                        Console.WriteLine($"  Starting new line size {size}");
                        FlushCurrentLine();
                        currentSize = size;
                    }
                    currentLine.FormProperties.Add(property);
                }
            }
            FlushCurrentFieldset();

            void FlushCurrentLine()
            {
                if(currentLine.FormProperties.Any()) {
                    currentFieldset.Lines.Add(currentLine);
                }
                Console.WriteLine("Adding new Line");
                currentLine = new FormLine(model);
            }

            void FlushCurrentFieldset()
            {
                FlushCurrentLine();
                if(currentFieldset.Lines.Any()) {
                    Fieldsets.Add(currentFieldset);
                }
                Console.WriteLine("Adding New Fieldset");
                currentFieldset = new FormFieldset();
                //currentLine = new FormLine(model);
            }
        }

        private static int PredictSize(DryProperty property)
        {
            if(property.Property.PropertyType == typeof(string)) {
                var length = property.MaxLength?.Length ?? 1000;
                if(length <= 50) {
                    return 1;
                }
                else if(length <= 100) {
                    return 2;
                }
                else if(length <= 200) {
                    return 3;
                }
                else {
                    return 4;
                }
            }
            return 1;
        }

        private async Task ExecuteAsync(CommandInfo command)
        {
            try {
                await command.ExecuteAsync(Model);
            }
            catch(DryException ex) {
                error = ex.UserMessage;
            }
            catch(Exception) {
                error = "Unrecognized error occurred";
            }
        }
    }
}
