
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.ExtraDry {
    public partial class DryInput<T> : OwningComponentBase, IDisposable {

        [Parameter]
        public T Model { get; set; }

        [Parameter]
        public PropertyDescription Property { get; set; }

        [Parameter]
        public string PropertyName { get; set; }

        [CascadingParameter]
        public EditMode EditMode { get; set; } = EditMode.Create;

        protected async override Task OnInitializedAsync()
        {
            Property ??= new PropertyDescription(typeof(T).GetProperty(PropertyName));
            if(Property.HasTextRepresentation == false) {
                await FetchLookupProviderOptions();
            }
        }

        private List<object> LookupProviderOptions { get; set; }

        private bool RulesAllowUpdate => (Property.Rules?.UpdateAction ?? UpdateAction.AllowChanges) == UpdateAction.AllowChanges;

        private bool Editable => EditMode == EditMode.Create || EditMode == EditMode.Update && RulesAllowUpdate;

        private bool ReadOnly => !Editable;

        private string Value => Property.DisplayValue(Model);

        private string validationMessage;

        private bool valid = true;

        private string CssClass => valid ? "valid" : "invalid";

        private string SizeClass => Property.Size.ToString().ToLowerInvariant();

        private bool ShowDescription { get; set; }

        private bool HasDescription => !string.IsNullOrWhiteSpace(Description);

        private async Task FetchLookupProviderOptions()
        {
            var untypedOptionProvider = typeof(IOptionProvider<>);
            var typedOptionProvider = untypedOptionProvider.MakeGenericType(Property.Property.PropertyType);
            var optionProvider = ScopedServices.GetService(typedOptionProvider);
            if(optionProvider != null) {
                var method = typedOptionProvider.GetMethod("ListAsync");
                dynamic task = method.Invoke(optionProvider, new object[] { Array.Empty<object>() });
                var optList = (await task) as ICollection;
                LookupProviderOptions = optList.Cast<object>().ToList();
            }
        }

        private string Description {
            get {
                if(Editable) {
                    return Property.Description + (Property.IsRequired ? " (required)" : "");
                }
                else {
                    return $"{Property.Description} (read&#8209;only)"; // non-breaking-hyphen.
                }
            }
        }

        private void ToggleDescription(MouseEventArgs args)
        {
            ShowDescription = !ShowDescription;
        }

        private void OnChange(ChangeEventArgs args)
        {
            Console.WriteLine("Changed");
            var value = args.Value;
            if(LookupProviderOptions != null && value is string strValue) {
                value = LookupProviderOptions.FirstOrDefault(e => e.ToString() == strValue);
            }
            Console.WriteLine($"Model: {Model} to Value: {value}");
            Property.SetValue(Model, value);
            Validate();
        }

        private void Validate()
        {
            var validator = new DataValidator();
            if(validator.ValidateProperties(Model, Property.Property.Name)) {
                validationMessage = "";
                valid = true;
            }
            else {
                validationMessage = validator.Errors.First().ErrorMessage;
                valid = false;
            }
        }


    }
}
