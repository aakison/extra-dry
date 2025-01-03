using System.Collections.ObjectModel;
using System.Reflection;

namespace ExtraDry.Server.DataWarehouse.Builder;

internal class EnumStats(Type enumType)
{
    public int DisplayNameMaxLength() => fields.Max(e => e.DisplayName.Length);

    public bool HasShortName() => fields.Any(e => e.ShortName != null);

    public int ShortNameMaxLength() => fields.Max(e => e.ShortName?.Length ?? 0);

    public bool HasDescription() => fields.Any(e => e.Description != null);

    public int DescriptionMaxLength() => fields.Max(e => e.Description?.Length ?? 0);

    public bool HasGroupName() => fields.Any(e => e.GroupName != null);

    public int GroupNameMaxLength() => fields.Max(e => e.GroupName?.Length ?? 0);

    public bool HasOrder() => fields.Any(e => e.Order != null);

    public ReadOnlyCollection<EnumFieldStats> Fields => new(fields);

    private readonly List<EnumFieldStats> fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(e => new EnumFieldStats(e))
        .ToList();

    internal class EnumFieldStats(FieldInfo field)
    {
        public FieldInfo Field { get; set; } = field;

        public DisplayAttribute? Display { get; set; } = field.GetCustomAttribute<DisplayAttribute>();

        public string DisplayName => Display?.Name ?? DataConverter.CamelCaseToTitleCase(Field.Name);

        public string? ShortName => Display?.ShortName;

        public string? Description => Display?.Description;

        public string? GroupName => Display?.GroupName;

        public int? Order => Display?.GetOrder();

        public int Value => (int)(Field.GetValue(null) ?? 0);
    }
}
