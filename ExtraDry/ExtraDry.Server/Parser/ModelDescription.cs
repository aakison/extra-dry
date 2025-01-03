using System.Collections.ObjectModel;
using System.Reflection;

namespace ExtraDry.Server.Internal;

internal class ModelDescription
{
    public ModelDescription(Type modelType)
    {
        GetReflectedModelProperties(modelType);
    }

    public Collection<FilterProperty> FilterProperties { get; } = [];

    public Collection<SortProperty> SortProperties { get; } = [];

    public Collection<StatisticsProperty> StatisticsProperties { get; } = [];

    public SortProperty? StabilizerProperty => multipleStabilizerProperties ? null : stabilizerProperty;

    /// <summary>
    /// Determines if a given property is treated as sortable. This applies to the constructions of
    /// expressions as well as to Swagger documention.
    /// </summary>
    /// <remarks>
    /// 1. Ignore properties with specific attributes.
    /// 2. Ignore properties that are comples objects (not value types or strings)
    /// 3. Ignore GUIDs
    /// 4. Ignore EF key properties
    /// 5. Ignore properties that don't have a setter.
    /// </remarks>
    public static bool IsSortable(PropertyInfo prop)
    {
        if(prop.GetCustomAttribute<SortAttribute>() is SortAttribute sa) {
            // override rules, use the explicity behavior.
            return sa.Type == SortType.Sortable;
        }
        // Note attributes are by name to avoid having to take dependency on EF.
        var disqualifyingAttributes = new string[] { "JsonIgnoreAttribute", "NotMappedAttribute", "KeyAttribute" };
        var ignore = prop.GetCustomAttributes().Any(e => disqualifyingAttributes.Any(f => f == e.GetType().Name));
        if(prop.Name == "Id" || prop.Name == $"{prop.DeclaringType?.Name}Id") {
            // EF convention for Key
            ignore = true;
        }
        if(prop.PropertyType == typeof(Guid)) {
            ignore = true;
        }
        if(!ignore && !prop.PropertyType.IsValueType && prop.PropertyType != typeof(string)) {
            ignore = true;
        }
        // Ignore properties that don't have a setter, they aren't in the database.
        if(prop.SetMethod == null) {
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

            var statistics = property.GetCustomAttribute<StatisticsAttribute>();
            if(statistics != null) {
                StatisticsProperties.Add(new StatisticsProperty(property, externalName, statistics.Stats));
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

    private SortProperty? stabilizerProperty;

    private bool multipleStabilizerProperties;
}
