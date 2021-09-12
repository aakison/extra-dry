using ExtraDry.Core;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ExtraDry.Server.Internal {

    internal class ModelDescription {

        public ModelDescription(Type modelType)
        {
            GetReflectedModelProperties(modelType);
        }

        public Collection<FilterProperty> FilterProperties { get; } = new Collection<FilterProperty>();

        private void GetReflectedModelProperties(Type modelType)
        {
            var properties = modelType.GetProperties();
            foreach(var property in properties) {
                var filter = property.GetCustomAttribute<FilterAttribute>();
                if(filter != null) {
                    FilterProperties.Add(new FilterProperty(property, filter));
                }
            }
        }

    }
}
