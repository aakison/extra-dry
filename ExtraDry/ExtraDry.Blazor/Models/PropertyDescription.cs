using ExtraDry.Blazor.Models.InputValueFormatters;

namespace ExtraDry.Blazor;

public class PropertyDescription
{

    public static PropertyDescription For(object model, string propertyName)
    {
        return For(model.GetType(), propertyName);
    }

    public static PropertyDescription For(Type modelType, string propertyName)
    {
        var propertyInfo = modelType.GetProperty(propertyName) 
            ?? throw new ArgumentException($"Property {propertyName} not found on {modelType.Name}");
        return new PropertyDescription(propertyInfo);
    }

    public PropertyDescription(PropertyInfo property)
    {
        Property = property;
        Display = Property.GetCustomAttribute<DisplayAttribute>();
        Format = Property.GetCustomAttribute<DisplayFormatAttribute>();
        Rules = Property.GetCustomAttribute<RulesAttribute>();
        MaxLength = Property.GetCustomAttribute<MaxLengthAttribute>();
        StringLength = Property.GetCustomAttribute<StringLengthAttribute>();
        IsRequired = Property.GetCustomAttribute<RequiredAttribute>() != null;
        Control = Property.GetCustomAttribute<ControlAttribute>();
        Filter = Property.GetCustomAttribute<FilterAttribute>();
        InputFormat = Property.GetCustomAttribute<InputFormatAttribute>();
        Sort = Property.GetCustomAttribute<SortAttribute>();
        
        FieldCaption = Display?.Name ?? DataConverter.CamelCaseToTitleCase(Property.Name);
        ColumnCaption = Display?.ShortName ?? DataConverter.CamelCaseToTitleCase(Property.Name);
        Description = Display?.Description;
        Order = Display?.GetOrder();
        HasDescription = !string.IsNullOrWhiteSpace(Description);
        Size = PredictSize();
        NullDisplayText = Format?.NullDisplayText ?? "";
        if(HasDiscreteValues) {
            var enumValues = Property.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach(var enumValue in enumValues) {
                var value = (int)enumValue.GetValue(null)!;
                var enumDisplay = enumValue.GetCustomAttribute<DisplayAttribute>();
                discreteDisplayAttributes.Add(value, enumDisplay);
            }
        }
        ++recursionDepth;
        if(recursionDepth < 10 && Rules?.CreateAction != RuleAction.Link && (Rules?.CreateAction == RuleAction.Allow || Rules?.UpdateAction == RuleAction.Allow || Rules?.CreateAction == RuleAction.IgnoreDefaults || Rules?.CreateAction == RuleAction.IgnoreDefaults)) {
            if(HasArrayValues) {
                var elementProperty = Property.PropertyType.SingleGenericType();
                ChildModel = new ViewModelDescription(elementProperty, this);
            }
            else if(Property.PropertyType.IsClass && Property.PropertyType != typeof(string)) {
                ChildModel = new ViewModelDescription(Property.PropertyType, this);
            }
        }
        --recursionDepth;
        AllowsNull = Property.PropertyType.IsGenericType && Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        PropertyType = AllowsNull
            ? Property.PropertyType.GetGenericArguments()[0] 
            : Property.PropertyType;

        Formatter = CreateFormatter();
    }

    /// <summary>
    /// It is possible that mapping through ChildModel's will create an infinite loop;
    /// Check and limit the number of layers, in practice a UI will be horrible if recursion is even 2-3 levels.
    /// </summary>
    private static int recursionDepth;

    public ViewModelDescription? ChildModel { get; private set; }

    /// <summary>
    /// When rendered as an input component, the label for the input field. This may be set in the 
    /// <see cref="DisplayAttribute.Name" />, or will default to a title-case version of the 
    /// property name.
    /// </summary>
    public string FieldCaption { get; set; }

    /// <summary>
    /// When rendered as a table colun, the label for the table header.  This may be set in the
    /// <see cref="DisplayAttribute.ShortName" />, or will default to a title-case version of the
    /// property name.
    /// </summary>
    public string ColumnCaption { get; set; }

    public DisplayAttribute? Display { get; }

    public DisplayFormatAttribute? Format { get; }

    public RulesAttribute? Rules { get; }

    public FilterAttribute? Filter { get; }

    public InputFormatAttribute? InputFormat { get; }

    public SortAttribute? Sort { get; }

    /// <summary>
    /// Use FieldLength instead.
    /// </summary>
    private MaxLengthAttribute? MaxLength { get; }

    /// <summary>
    /// Use FieldLength instead.
    /// </summary>
    private StringLengthAttribute? StringLength { get; }

    public ControlAttribute? Control { get; }

    public string? Description { get; }

    /// <summary>
    /// The display order, null if it is not present
    /// </summary>
    public int? Order { get; set; }

    public bool IsRequired { get; }

    public bool HasDescription { get; }

    public PropertyInfo Property { get; set; }

    /// <summary>
    /// The calculated size of the property.
    /// </summary>
    public PropertySize Size { get; private set; }

    public string NullDisplayText { get; set; }

    public string DisplayValue(object? item)
    {
        if(item == null) {
            return string.Empty;
        }
        try {
            var value = Property?.GetValue(item);
            if(value == null) {
                return Format?.NullDisplayText ?? "null";
            }
            if(HasDiscreteValues && discreteDisplayAttributes.TryGetValue((int)value, out var display)) {
                value = display?.GetName() ?? value;
            }
            var format = Format?.DataFormatString ?? "{0}";
            return string.Format(CultureInfo.InvariantCulture, format, value);
        }
        catch(Exception ex) {
            return ex.Message;
        }
    }

    public object? GetValue(object? item) => item == null ? null : Property?.GetValue(item);

    public int GetDiscreteSortOrder(object item)
    {
        if(item == null) {
            return 0;
        }
        if(HasDiscreteValues) {
            var valueObj = GetValue(item);
            if(valueObj == null) {
                return 0;
            }
            var value = (int)valueObj;
            if(discreteDisplayAttributes.TryGetValue(value, out var display)) {
                return display?.GetOrder() ?? value;
            }
            return value;
        }
        else {
            return 0;
        }
    }

    public ControlType ControlType => Control?.Type ?? ControlType.BestMatch;

    public string Icon => Control?.Icon ?? "";

    public string CaptionTemplate => Control?.CaptionTemplate ?? "";

    public void SetValue(object item, object? value)
    {
        if(HasArrayValues) {
            throw new InvalidOperationException("Can only set values to properties that are not collections, use AddValue instead.");
        }
        Property?.SetValue(item, Unformat(value));
    }

    public void AddValue(object item, object value)
    {
        var propertyList = GetAsCollection(item);
        propertyList?.Add(value);
    }

    public void RemoveValue(object item, object value)
    {
        var propertyList = GetAsCollection(item);
        propertyList?.Remove(value);
    }

    private IList GetAsCollection(object item)
    {
        if(!HasArrayValues) {
            throw new InvalidOperationException("Can only access values on property that are collections.");
        }
        var propertyObject = GetValue(item);
        if(propertyObject == null) {
            propertyObject = Activator.CreateInstance(Property.PropertyType);
            Property?.SetValue(item, propertyObject);
        }
        var propertyList = propertyObject as IList;
        if(propertyList == null) {
            // TODO: Change to logging, better behavior here?
            Console.WriteLine("Not castable to IList");
        }
        return propertyList ?? Array.Empty<object>();
    }

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

    public string ItemDisplayClass(object? item)
    {
        var typeClass = DisplayClass;
        var value = Property?.GetValue(item);

        if(value == null) {
            typeClass = $"{typeClass} null";
        }

        return typeClass;
    }

    public bool HasDiscreteValues {
        get {
            if(Property.PropertyType.IsEnum) {
                return true;
            }
            // TODO: Check about other discrete options, like valid values in a database.
            return false;
        }
    }

    public bool HasArrayValues => typeof(IEnumerable).IsAssignableFrom(Property.PropertyType) && !typeof(string).IsAssignableFrom(Property.PropertyType);

    private readonly Dictionary<int, DisplayAttribute?> discreteDisplayAttributes = [];

    public bool HasNumericRepresentation {
        get {
            var types = new List<Type> { typeof(decimal), typeof(decimal?), typeof(int), typeof(int?), typeof(float) };
            return types.Contains(Property.PropertyType);
        }
    }

    public bool HasTextRepresentation {
        get {
            var types = new List<Type> { typeof(string), typeof(Uri) };
            return types.Contains(Property.PropertyType) || Property.PropertyType.IsEnum;
        }
    }

    public bool HasDateTimeRepresentation {
        get {
            var types = new List<Type> { typeof(DateTime), typeof(DateTime?), typeof(DateOnly), typeof(DateOnly?), typeof(TimeOnly), typeof(TimeOnly?) };
            return types.Contains(InputType);
        }
    }

    public bool HasBooleanValues {
        get {
            var types = new List<Type> { typeof(bool), typeof(bool?) };
            return types.Contains(Property.PropertyType);
        }
    }

    public bool HasFreshnessRepresentation => Property.PropertyType == typeof(UserTimestamp);

    public IList<ValueDescription> GetDiscreteValues()
    {
        var values = new List<ValueDescription>();
        var enumValues = Property.PropertyType.GetEnumValues();
        foreach(var value in enumValues) {
            if(value != null) {
                var memberInfo = Property.PropertyType.GetMember(value.ToString()!).First();
                var valueDescription = new ValueDescription(value, memberInfo);
                if(valueDescription.AutoGenerate) {
                    values.Add(valueDescription);
                }
            }
        }
        return values;
    }

    private object? Unformat(object? value)
    {
        if(PropertyType.IsEnum) {
            return Enum.Parse(Property.PropertyType, value?.ToString() ?? "");
        }
        else if(PropertyType == typeof(string)) {
            return value?.ToString();
        }
        else if(PropertyType == typeof(decimal) || PropertyType == typeof(int)) {
            if(Formatter.TryParse(value?.ToString() ?? "", out var result)) {
                return result;
            }
        }
        else {
            // fallback for object types
            return value;
        }
        return null; // fallback for parsing errors.
    }

    public int? FieldLength => StringLength?.MaximumLength ?? MaxLength?.Length;

    /// <summary>
    /// Gets the type of the property.  If the property is a nullable type (e.g. 
    /// Nullable&lt;DateTime&gt; or DateTime?) then just the inner type is returned (e.g. DateTime).
    /// See <see cref="AllowsNull" /> for nullability.
    /// </summary>
    public Type PropertyType { get; }

    /// <summary>
    /// Gets the nullability of the property.  See <see cref="PropertyType"/> for the base type.
    /// </summary>
    public bool AllowsNull { get; }

    /// <summary>
    /// Returns the type of the property as it should be displayed to users during input.  This
    /// may vary slightly (e.g. PropertyType is DateTime, where the InputType is DateOnly).
    /// </summary>
    public Type InputType => InputFormat?.DataTypeOverride ?? PropertyType;

    private PropertySize PredictSize()
    {
        if(Property.PropertyType == typeof(string)) {
            var overrideSize = InputFormat?.SizeOverride ?? PropertySize.Unset;
            if(overrideSize != PropertySize.Unset) {
                return overrideSize;
            }
            var length = FieldLength ?? 1000;
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

    private InputValueFormatter CreateFormatter()
    {
        return (AllowsNull, PropertyType) switch {
            (false, Type t) when t == typeof(decimal) => new DecimalValueFormatter(this),
            (true, Type t) when t == typeof(decimal) => new NullableDecimalValueFormatter(this),
            (false, Type t) when t == typeof(int) => new IntValueFormatter(this),
            (true, Type t) when t == typeof(int) => new NullableIntValueFormatter(this),
            _ => new IdentityValueFormatter(this)
        };
    }

    public InputValueFormatter Formatter { get; set; }
}
