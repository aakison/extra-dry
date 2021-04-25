using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Blazor.ExtraDry {
    public class DryProperty {

        public DryProperty(PropertyInfo property)
        {
            var display = property.GetCustomAttribute<DisplayAttribute>();
            var format = property.GetCustomAttribute<DisplayFormatAttribute>();
            var required = property.GetCustomAttribute<RequiredAttribute>();
            var rules = property.GetCustomAttribute<RulesAttribute>();
            var length = property.GetCustomAttribute<MaxLengthAttribute>();
            var header = property.GetCustomAttribute<HeaderAttribute>();
            Rules = rules;
            MaxLength = length;
            FieldCaption = display?.Name ?? property.Name;
            ColumnCaption = display?.ShortName ?? property.Name;
            Header = header;
            Display = display;
            Format = format;
            Property = property;
            IsRequired = required != null;
            Description = display?.Description;
            HasDescription = !string.IsNullOrWhiteSpace(Description);
            if(HasDiscreteValues) {
                var enumValues = property.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach(var enumValue in enumValues) {
                    var value = (int)enumValue.GetValue(null);
                    var enumDisplay = enumValue.GetCustomAttribute<DisplayAttribute>();
                    discreteDisplayAttributes.Add(value, enumDisplay);
                }
            }
            ++recursionDepth;
            if(recursionDepth < 10 && Rules?.CreateAction == CreateAction.CreateNew) {
                ChildModel = new ViewModelDescription(Property.PropertyType, this);
            }
            --recursionDepth;
        }

        /// <summary>
        /// It is possible that mapping through ChildModel's will create an infinite loop;
        /// Check and limit the number of layer, in practice a UI will be horrible if recursion is even 2-3 levels.
        /// </summary>
        private static int recursionDepth = 0;

        public ViewModelDescription ChildModel { get; private set; }

        public string FieldCaption { get; set; }

        public string ColumnCaption { get; set; }

        public HeaderAttribute Header { get; }

        public DisplayAttribute Display { get; }

        public DisplayFormatAttribute Format { get; }

        public RulesAttribute Rules { get; }

        public MaxLengthAttribute MaxLength { get; }

        public string Description { get; }

        public bool IsRequired { get; }

        public bool HasDescription { get; }

        public PropertyInfo Property { get; set; }

        public string DisplayValue(object item)
        {
            if(item == null) {
                return string.Empty;
            }
            var value = Property?.GetValue(item);
            if(HasDiscreteValues && discreteDisplayAttributes.ContainsKey((int)value)) {
                value = discreteDisplayAttributes[(int)value].GetName() ?? value;
            }
            var format = Format?.DataFormatString ?? "{0}";
            return string.Format(CultureInfo.InvariantCulture, format, value);
        }

        public object GetValue(object item) => item == null ? null : Property?.GetValue(item);

        public int GetDiscreteSortOrder(object item)
        {
            if(HasDiscreteValues) {
                var value = (int)GetValue(item);
                if(discreteDisplayAttributes.ContainsKey(value)) {
                    var display = discreteDisplayAttributes[value];
                    return display.GetOrder() ?? value;
                }
                return value;
            }
            else {
                return 0;
            }
        }

        public void SetValue(object item, object value) => Property?.SetValue(item, Unformat(value));

        public string DisplayClass {
            get {
                string typeClass = "";
                if(Property.PropertyType == typeof(decimal)) {
                    typeClass = "decimal";
                }
                else if(HasDiscreteValues) {
                    typeClass = "enum";
                }
                return $"{typeClass} {Property.Name.ToLowerInvariant()}";
            }
        }

        public bool HasDiscreteValues
        {
            get {
                if(Property.PropertyType.IsEnum) {
                    return true;
                }
                // TODO: Check about other discrete options, like valid values in a database.
                return false;
            }
        }

        private readonly Dictionary<int, DisplayAttribute> discreteDisplayAttributes = new();

        public bool HasTextRepresentation {
            get {
                var types = new List<Type> { typeof(decimal), typeof(decimal?), typeof(string) };
                return types.Contains(Property.PropertyType);
            }
        }

        public bool HasBooleanValues {
            get {
                var types = new List<Type> { typeof(bool), typeof(bool?) };
                return types.Contains(Property.PropertyType);
            }
        }

        public IList<KeyValuePair<object, string>> GetDiscreteValues()
        {
            var pairs = new List<KeyValuePair<object, string>>();
            var enumValues = Property.PropertyType.GetEnumValues();
            foreach(var value in enumValues) {
                var memberInfo = Property.PropertyType.GetMember(value.ToString()).First();
                var display = memberInfo.GetCustomAttribute<DisplayAttribute>();
                var name = display?.Name ?? memberInfo.Name;
                pairs.Add(new KeyValuePair<object, string>(value, name));
            }
            return pairs;
        }

        private object Unformat(object value)
        {
            if(Property.PropertyType.IsEnum) {
                return Enum.Parse(Property.PropertyType, value.ToString());
            }
            else if(Property.PropertyType == typeof(string)) {
                return value?.ToString();
            }
            else if(Property.PropertyType == typeof(decimal)) {
                if(value is string strValue) {
                    if(Format?.DataFormatString?.Contains(":P") ?? false) {
                        return PercentageToDecimal(strValue) ?? 0m;
                    }
                    else {
                        return StringToDecimal(strValue) ?? 0m;
                    }
                }
                else {
                    throw new NotImplementedException();
                }
            }
            else if(Property.PropertyType == typeof(decimal?)) {
                if(value is string strValue) {
                    if(Format?.DataFormatString?.Contains(":P") ?? false) {
                        return PercentageToDecimal(strValue);
                    }
                    else {
                        return StringToDecimal(strValue);
                    }
                }
                else {
                    throw new NotImplementedException();
                }
            }
            else {
                // fallback for object types
                return value;
            }
        }

        private static decimal? PercentageToDecimal(string percent) =>
            decimal.TryParse(percent.Replace("%", ""), out var result) ? result / 100m : null;

        private static decimal? StringToDecimal(string value) =>
            decimal.TryParse(value, out var result) ? result : null;

    }
}
