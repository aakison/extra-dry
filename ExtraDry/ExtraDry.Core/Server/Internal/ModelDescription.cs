#nullable enable

using ExtraDry.Core;
using ExtraDry.Server.Internal;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ExtraDry.Server.Internal {

    internal class ModelDescription {

        public ModelDescription(Type modelType)
        {
            ModelType = modelType;
            GetReflectedModelProperties(modelType);
        }

        public Type ModelType { get; }

        public Collection<FilterProperty> FilterProperties { get; } = new Collection<FilterProperty>();

        private void GetReflectedModelProperties(Type modelType)
        {
            if(modelType == null) {
                return;
            }
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
