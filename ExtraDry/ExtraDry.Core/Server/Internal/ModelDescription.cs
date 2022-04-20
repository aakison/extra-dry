#nullable enable

using ExtraDry.Core;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ExtraDry.Server.Internal {

    internal class ModelDescription {
        private const string DuplicateKeyMessage = "Sort requires that a single EF key is well defined to stabalize the sort, composite keys are not supported.  Manually specify a Stabilizer in the FilterQuery, or use a single [Key] attribute.";
        private const string MissingStabilizerMessage = "Sort requires that an EF key is uniquely defined to stabalize the sort, even if another sort property is present.  Create a unique key following EF conventions or specify a Stabilizer in the FilterQuery.";
        private const string UserMessage = "Unable to Sort. {0}";

        public ModelDescription(Type modelType)
        {
            GetReflectedModelProperties(modelType);
        }

        public Collection<FilterProperty> FilterProperties { get; } = new Collection<FilterProperty>();
        public Collection<SortProperty> SortProperties { get; } = new Collection<SortProperty>();
        public SortProperty StabilizerProperty { get; private set; }

        private void GetReflectedModelProperties(Type modelType)
        {
            var properties = modelType.GetProperties();
            SortProperty? stabilizerPropertyByConvention = default;
            foreach(var property in properties) {
                var externalName = ExternalName(property);

                var filter = property.GetCustomAttribute<FilterAttribute>();
                if(filter != null) {
                    FilterProperties.Add(new FilterProperty(property, filter));
                }

                if(IsSortable(property)) {
                    SortProperties.Add(new SortProperty(property, externalName));
                }

                var keyProperty = property.GetCustomAttribute<KeyAttribute>();
                if(keyProperty != default) {
                    if(StabilizerProperty == default) {
                        StabilizerProperty = new SortProperty(property, externalName);
                    }
                    else {
                        throw new DryException(DuplicateKeyMessage, string.Format(UserMessage, "0x0F3F241D"));
                    }
                }

                if(property.Name == "Id") {
                    stabilizerPropertyByConvention = new SortProperty(property, externalName);
                }
                else if(stabilizerPropertyByConvention == default && property.Name == $"{modelType.Name}Id") {
                    stabilizerPropertyByConvention = new SortProperty(property, externalName);
                }
            }
            if(StabilizerProperty == default) {
                StabilizerProperty = stabilizerPropertyByConvention ?? throw new DryException(MissingStabilizerMessage, string.Format(UserMessage, "0x0F3F241C"));
            }
        }

        private static string ExternalName(PropertyInfo property)
        {
            var propAttrib = property.GetCustomAttributes().FirstOrDefault(e => e.GetType().Name == "JsonPropertyNameAttribute");
            if(propAttrib != default && propAttrib is JsonPropertyNameAttribute jsonPropAttrib) {
                return jsonPropAttrib.Name;
            }
            return property.Name;
        }

        public static bool IsSortable(PropertyInfo prop)
        {
            // By name to avoid having to take dependency on EF and/or Newtonsoft.
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
    }
}
