namespace ExtraDry.Core;

//// In FMI:
//public class CustomFieldSchema : IEntity
//{
//    [Key]
//    public int Id { get; set; }

//    public ExpandoSchema Schema { get; set; }

//    public VersionInfo VersionInfo { get; set; } = new();
//}

/// <summary>
/// Represents the structure of a tag that can be added to a model object and the associated elements for working with
/// those tags.
/// The tags are the most visible part of the system and to the user are made up of fields.
/// Additionally, templates have a workflow that defines the lifecycle of each tag.
/// It has forms which define how to display the tag, while only one form will be used at a time, multiple forms are
/// available to define different layouts on different form factors.
/// </summary>
/// .ToJson() in EF...
public class ExpandoSchema {

    /// <summary>
    /// The target type of the class that the <see cref="ExpandoSchema"/> provides extentions fields for.
    /// </summary>
    [Required]
    public string TargetType { get; set; } = string.Empty;

    public List<ExpandoSection> Sections { get; private set;} = new();

    public ExpandoState State { get; set; } = ExpandoState.Active;

    /// <summary>
    /// A collection of TemplateCondition which can be utilised in the
    /// <see cref="RuleSet"/> of a <see cref="ExpandoField"/>.
    /// </summary>
    //public List<Condition> Conditions { get; private set; }
}
