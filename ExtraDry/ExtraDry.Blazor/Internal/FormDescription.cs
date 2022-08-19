#nullable enable

using System.Collections;
using System.Collections.ObjectModel;

namespace ExtraDry.Blazor.Internal;

/// <summary>
/// Represents the layout of a form for a given ViewModel type and Model object.
/// This will link to the appropriate elements in the model or its contained sub-objects.
/// </summary>
internal class FormDescription {

    public FormDescription(ViewModelDescription description, object model)
    {
        ViewModelDescription = description;
        ExtendProperties(description.FormProperties, "", FormGroupType.Properties, model);
        BuildFieldsets();
    }

    public ViewModelDescription ViewModelDescription { get; init; }

    public List<FormFieldset> Fieldsets { get; } = new();

    private List<ExtendedProperty> ExtendedProperties { get; set; } = new();

    private void BuildFieldsets()
    {
        int lineCapacity = 4;
        object? lastModel = null;
        foreach(var property in ExtendedProperties) {
            var fieldset = Fieldsets.LastOrDefault(e => string.Equals(e.Legend, property.FieldsetTitle, StringComparison.OrdinalIgnoreCase));
            if(fieldset == null) {
                fieldset = new FormFieldset(property.FieldsetTitle, WebId.ToWebId(property.FieldsetTitle));
                Fieldsets.Add(fieldset);
            }
            var group = fieldset.Groups.LastOrDefault(e => e.Type == property.GroupType && e.Target == property.Target);
            if(group == null) {
                group = new FormGroup(property.Target) { Type = property.GroupType, ParentTarget = property.ParentTarget };
                fieldset.Groups.Add(group);
            }
            var line = group.Lines.LastOrDefault();
            if(line == null || lineCapacity < (int)property.Length) {
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
            lineCapacity -= (int)property.Length;
            lastModel = property.Target;
        }
    }

    private void ExtendProperties(Collection<PropertyDescription> properties, string fieldsetName, FormGroupType formGroup, object? model, object? parentModel = null)
    {
        foreach(var property in properties) {
            var groupName = property.Display?.GroupName ?? "default";
            if(groupName != string.Empty) {
                fieldsetName = groupName;
            }
            if(property.ChildModel != null) {
                if(!property.HasArrayValues) {
                    var submodel = property.GetValue(model);
                    ExtendProperties(property.ChildModel.FormProperties, fieldsetName, FormGroupType.Object, submodel);
                }
                else {
                    var collection = property.GetValue(model) as ICollection;
                    try {
                        var type = property.Property.PropertyType.SingleGenericType();
                        foreach(var submodel in collection!) {
                            ExtendProperties(property.ChildModel.FormProperties, fieldsetName, FormGroupType.Element, submodel, collection);
                        }
                        // TODO: different for IEnumerable?
                        if(collection is IList list && type.HasDefaultConstructor()) {
                            ExtendedProperties.Add(new ExtendedProperty(property, list) {
                                FieldsetTitle = fieldsetName,
                                GroupType = FormGroupType.Commands,
                                Length = PropertySize.Jumbo,
                                CommandRow = true,
                            });
                        }
                    }
                    catch(DryException ex) {
                        Console.WriteLine(ex.Message);
                        // TODO: Determine correct error behavior.
                    }
                }
            }
            else if(model != null) {
                ExtendedProperties.Add(new ExtendedProperty(property, model) { 
                    FieldsetTitle = fieldsetName,
                    GroupType = formGroup,
                    Length = property.Size,
                    ParentTarget = parentModel,
                });
            }
        }
    }


    private class ExtendedProperty {
        public ExtendedProperty(PropertyDescription property, object target) {
            Property = property;
            Target = target;
        }

        public string FieldsetTitle { get; set; } = string.Empty;
        public FormGroupType GroupType { get; set; }
        public PropertySize Length { get; set; }
        public PropertyDescription? Property { get; set; }
        public object Target { get; set; }
        public object? ParentTarget { get; set; }
        public bool CommandRow { get; set; }
    }

}
