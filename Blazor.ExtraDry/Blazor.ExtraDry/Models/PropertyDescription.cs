using Blazor.ExtraDry.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Blazor.ExtraDry {
    public class PropertyDescription {

        public PropertyDescription(PropertyInfo property)
        {
            Property = property;
            Display = Property.GetCustomAttribute<DisplayAttribute>();
            Format = Property.GetCustomAttribute<DisplayFormatAttribute>();
            Rules = Property.GetCustomAttribute<RulesAttribute>();
            Header = Property.GetCustomAttribute<HeaderAttribute>();
            MaxLength = Property.GetCustomAttribute<MaxLengthAttribute>();
            IsRequired = Property.GetCustomAttribute<RequiredAttribute>() != null;
            Control = Property.GetCustomAttribute<ControlAttribute>();
            FieldCaption = Display?.Name ?? Property.Name;
            ColumnCaption = Display?.ShortName ?? Property.Name;
            Description = Display?.Description;
            HasDescription = !string.IsNullOrWhiteSpace(Description);
            Size = PredictSize();
            if(HasDiscreteValues) {
                var enumValues = Property.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static);
                foreach(var enumValue in enumValues) {
                    var value = (int)enumValue.GetValue(null);
                    var enumDisplay = enumValue.GetCustomAttribute<DisplayAttribute>();
                    discreteDisplayAttributes.Add(value, enumDisplay);
                }
            }
            ++recursionDepth;
            if(recursionDepth < 10 && Rules?.CreateAction == CreateAction.CreateNew) { // Create new signals to recurse
                if(HasArrayValues) {
                    var elementProperty = Property.PropertyType.SingleGenericType();
                    ChildModel = new ViewModelDescription(elementProperty, this);
                }
                else {
                    ChildModel = new ViewModelDescription(Property.PropertyType, this);
                }
            }
            --recursionDepth;
        }

        /// <summary>
        /// It is possible that mapping through ChildModel's will create an infinite loop;
        /// Check and limit the number of layers, in practice a UI will be horrible if recursion is even 2-3 levels.
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

        public ControlAttribute Control { get; }

        public string Description { get; }

        public bool IsRequired { get; }

        public bool HasDescription { get; }

        public PropertyInfo Property { get; set; }

        public PropertySize Size { get; set; }

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

        public ControlType ControlType => Control?.Type ?? ControlType.BestMatch;

        public string IconTemplate => Control?.IconTemplate ?? "";

        public string CaptionTemplate => Control?.CaptionTemplate ?? "";

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

        public bool HasArrayValues => typeof(IEnumerable).IsAssignableFrom(Property.PropertyType) && !typeof(string).IsAssignableFrom(Property.PropertyType);

        private readonly Dictionary<int, DisplayAttribute> discreteDisplayAttributes = new();

        public bool HasTextRepresentation {
            get {
                var types = new List<Type> { typeof(decimal), typeof(decimal?), typeof(string), typeof(Uri) };
                return types.Contains(Property.PropertyType);
            }
        }

        public bool HasBooleanValues {
            get {
                var types = new List<Type> { typeof(bool), typeof(bool?) };
                return types.Contains(Property.PropertyType);
            }
        }

        public IList<ValueDescription> GetDiscreteValues()
        {
            var values = new List<ValueDescription>();
            var enumValues = Property.PropertyType.GetEnumValues();
            foreach(var value in enumValues) {
                var memberInfo = Property.PropertyType.GetMember(value.ToString()).First();
                var valueDescription = new ValueDescription(value, memberInfo);
                values.Add(valueDescription);
            }
            return values;
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

        private PropertySize PredictSize()
        {
            if(Property.PropertyType == typeof(string)) {
                var length = MaxLength?.Length ?? 1000;
                if(length <= 25) {
                    return PropertySize.Small;
                }
                else if(length <= 50) {
                    return PropertySize.Medium;
                }
                else if(length <= 100) {
                    return PropertySize.Large;
                }
                else {
                    return PropertySize.Jumbo;
                }
            }
            return PropertySize.Small;
        }

        private static decimal? PercentageToDecimal(string percent) =>
            decimal.TryParse(percent.Replace("%", ""), out var result) ? result / 100m : null;

        private static decimal? StringToDecimal(string value) =>
            decimal.TryParse(value, out var result) ? result : null;

    }
}
