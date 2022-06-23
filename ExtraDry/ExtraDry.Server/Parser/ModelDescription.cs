#nullable enable

using System.Collections.ObjectModel;
using System.Reflection;

namespace ExtraDry.Server.Internal;

internal class ModelDescription {

    public ModelDescription(Type modelType)
    {
        GetReflectedModelProperties(modelType);
    }

    public Collection<FilterProperty> FilterProperties { get; } = new Collection<FilterProperty>();

    public Collection<SortProperty> SortProperties { get; } = new Collection<SortProperty>();

    public SortProperty? StabilizerProperty => multipleStabilizerProperties ? null : stabilizerProperty;

    public static bool IsSortable(PropertyInfo prop)
    {
        // By name to avoid having to take dependency on EF.
        var disqualifyingAttributes = new string[] { "JsonIgnoreAttribute", "NotMappedAttribute", "KeyAttribute" };
        var ignore = prop.GetCustomAttributes().Any(e => disqualifyingAttributes.Any(f => f == e.GetType().Name));
        if(prop.Name == "Id" || prop.Name == $"{prop.DeclaringType?.Name}Id") {
            // EF convention for Key
            ignore = true;
        }
        if(!ignore && !prop.PropertyType.IsValueType && prop.PropertyType != typeof(string)) {
            ignore = true;
        }
        return !ignore;
    }

    private static string ExternalName(PropertyInfo property)
    {
        var propAttrib = property.GetCustomAttributes().FirstOrDefault(e => e.GetType().Name == "JsonPropertyNameAttribute");
        if(propAttrib != default && propAttrib is JsonPropertyNameAttribute jsonPropAttrib) {
            return $"{jsonPropAttrib.Name[..1].ToUpperInvariant()}{jsonPropAttrib.Name[1..]}";
        }
        return property.Name;
    }

    private void GetReflectedModelProperties(Type modelType)
    {
        var properties = modelType.GetProperties();
        SortProperty? stabilizerPropertyByConvention = default;
        foreach(var property in properties) {
            var externalName = ExternalName(property);

            var filter = property.GetCustomAttribute<FilterAttribute>();
            if(filter != null) {
                FilterProperties.Add(new FilterProperty(property, filter, externalName));
            }

            if(IsSortable(property)) {
                SortProperties.Add(new SortProperty(property, externalName));
            }

            var keyProperty = property.GetCustomAttribute<KeyAttribute>();
            if(keyProperty != default) {
                if(stabilizerProperty == default) {
                    stabilizerProperty = new SortProperty(property, externalName);
                }
                else {
                    multipleStabilizerProperties = true;
                }
            }

            if(property.Name == "Id") {
                stabilizerPropertyByConvention = new SortProperty(property, externalName);
            }
            else if(stabilizerPropertyByConvention == default && property.Name == $"{modelType.Name}Id") {
                stabilizerPropertyByConvention = new SortProperty(property, externalName);
            }
        }
        if(stabilizerProperty == default) {
            stabilizerProperty = stabilizerPropertyByConvention;
        }
    }

    private SortProperty? stabilizerProperty = null;

    private bool multipleStabilizerProperties;

}
