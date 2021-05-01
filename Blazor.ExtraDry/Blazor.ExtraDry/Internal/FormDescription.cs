#nullable enable

using Blazor.ExtraDry.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blazor.ExtraDry.Internal {

    /// <summary>
    /// Represents the layout of a form for a given ViewModel type and Model object.
    /// This will link to the appropriate elements in the model or its contained sub-objects.
    /// </summary>
    internal class FormDescription {

        public FormDescription(ViewModelDescription description, object model)
        {
            ViewModelDescription = description;
            ExtendProperties(description.FormProperties, ColumnType.Primary, "", FormGroupType.Properties, model);
            BuildColumns();
        }

        public ViewModelDescription ViewModelDescription { get; init; }

        public List<FormColumn> Columns { get; } = new();

        private List<ExtendedProperty> ExtendedProperties { get; set; } = new();

        private void BuildColumns()
        {
            int lineCapacity = 4;
            object? lastModel = null;
            foreach(var property in ExtendedProperties) {
                var column = Columns.LastOrDefault(e => e.Type == property.ColumnType);
                if(column == null) {
                    column = new FormColumn { Type = property.ColumnType };
                    Columns.Add(column);
                }
                var fieldset = column.Fieldsets.LastOrDefault(e => string.Equals(e.Legend, property.FieldsetTitle, StringComparison.OrdinalIgnoreCase));
                if(fieldset == null) {
                    fieldset = new FormFieldset { Legend = property.FieldsetTitle };
                    column.Fieldsets.Add(fieldset);
                }
                var group = fieldset.Groups.LastOrDefault(e => e.Type == property.GroupType && e.Target == property.Target);
                if(group == null) {
                    group = new FormGroup(property.Target) { Type = property.GroupType };
                    fieldset.Groups.Add(group);
                }
                var line = group.Lines.LastOrDefault();
                if(line == null || lineCapacity < property.Length) {
                    line = new FormLine(property.Target);
                    group.Lines.Add(line);
                    lineCapacity = 4;
                }
                if(property.CommandRow) {
                    if(property.Target is IList list) {
                        line.Commands.Add(FormCommand.AddNew);
                    }
                }
                else if(property.Property != null) {
                    line.FormProperties.Add(property.Property);
                }
                lineCapacity -= property.Length;
                lastModel = property.Target;
            }
        }

        private void ExtendProperties(Collection<DryProperty> properties, ColumnType columnType, string fieldsetName, FormGroupType formGroup, object model)
        {
            foreach(var property in properties) {
                Console.WriteLine($"property: {property.Property.Name}, header: {property.Header?.Title}");
                if(property.Header != null) {
                    fieldsetName = property.Header.Title;
                    columnType = property.Header.Column;
                }
                if(property.ChildModel != null) {
                    if(!property.HasArrayValues) {
                        var submodel = property.GetValue(model);
                        ExtendProperties(property.ChildModel.FormProperties, columnType, fieldsetName, FormGroupType.Object, submodel);
                    }
                    else {
                        var collection = property.GetValue(model) as ICollection;
                        try {
                            var type = property.Property.PropertyType.SingleGenericType();
                            foreach(var submodel in collection!) {
                                ExtendProperties(property.ChildModel.FormProperties, columnType, fieldsetName, FormGroupType.Element, submodel);
                            }
                            // TODO: different for IEnumerable?
                            if(collection is IList list && type.HasDefaultConstructor()) {
                                ExtendedProperties.Add(new ExtendedProperty(property, list) {
                                    FieldsetTitle = fieldsetName,
                                    ColumnType = columnType,
                                    GroupType = FormGroupType.Commands,
                                    Length = 4,
                                    CommandRow = true,
                                });
                            }
                        }
                        catch(DryException ex) {
                            Console.WriteLine(ex.Message);
                            // TODO:
                        }
                    }
                }
                else {
                    var size = PredictSize(property);
                    ExtendedProperties.Add(new ExtendedProperty(property, model) { 
                        FieldsetTitle = fieldsetName,
                        ColumnType = columnType,
                        GroupType = formGroup,
                        Length = size,
                    });
                }
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


        private class ExtendedProperty {
            public ExtendedProperty(DryProperty property, object target) {
                Property = property;
                Target = target;
            }

            public ColumnType ColumnType { get; set; }
            public string FieldsetTitle { get; set; } = string.Empty;
            public FormGroupType GroupType { get; set; }
            public int Length { get; set; }
            public DryProperty? Property { get; set; }
            public object Target { get; set; }
            public bool CommandRow { get; set; }
        }

    }
}
