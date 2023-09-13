namespace ExtraDry.Core;

/// <summary>
/// Represents the structure of a tag that can be added to a model object and the associated elements for working with
/// those tags.
/// The tags are the most visible part of the system and to the user are made up of fields.
/// Additionally, templates have a workflow that defines the lifecycle of each tag.
/// It has forms which define how to display the tag, while only one form will be used at a time, multiple forms are
/// available to define different layouts on different form factors.
/// </summary>
public class Template {

    /// <summary>
    /// Creates a new template for defining the fields, workflow and display structures of tags.
    /// </summary>
    public Template()
    {
        Initialize();
    }

    private void Initialize()
    {
        Id = Guid.NewGuid();

        Fields = new FieldCollection {
            OnInsert = field => field.Parent = this,
            OnRemove = field => field.Parent = null
        };
        Partitions = new PartitionCollection();
        Conditions = new TemplateConditionCollection();

        IsDocument = false;
    }

    /// <summary>
    /// The identifier of the <see cref="Template"/>
    /// this can optionally be use in lieu of <see cref="Id"/>.
    /// </summary>

    [Required]
    [RegularExpression(ValidationExpression.IdentifierRegex, ErrorMessage = ValidationExpression.IdentifierMessage)]
    [StringLength(ValidationExpression.IdentifierMaxLength)]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// The name of the template
    /// this can optionally be use in lieu of { Id }.
    ///
    /// XmlSerializer no longer serializes properties that are marked with the Obsolete attribute.
    /// This is breaking current Capture XML configs.
    /// </summary>
    //[Obsolete("Use DisplayName.")]
    
    public string Name { get; set; }

    
    public bool IsDocument { get; set; }

    #region IVersionable

    /// <summary>
    /// The unique ID for the object, which defaults to a new globally unique ID.
    /// this can optionally be use in lieu of { Name }.
    /// </summary>
    
    public Guid Id { get; set; }

    /// <summary>
    /// The date that this object was last updated (in UTC), typically this is the date persisted in the database.
    /// It is not dynamically updated when this object is edited.
    /// </summary>
    public DateTime VersionDate { get; set; }

    /// <summary>
    /// The System/user that last updated the object
    /// </summary>
    public string VersionBy { get; set; }

    /// <summary>
    /// The date and time this object was created (in UTC).
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// The System/user that last created the object
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Indicates that this object is active or inactive.
    /// I.E. Soft delete.
    /// </summary>
    public bool IsActive { get; set; }

    #endregion IVersionable

    /// <summary>
    /// The name of the template, this will be displayed to the user and can be any printable string.
    /// </summary>
    
    public string DisplayName { get; set; }

    /// <summary>
    /// The definition of the title for the tags within this template.
    /// This uses the tag field pattern which is descibed in `Tag.ToString(string format)`
    /// </summary>
    
    public string TitlePattern { get; set; }

    /// <summary>
    /// The definition of the description for the tags within this template.
    /// This uses the tag field pattern which is descibed in `Tag.ToString(string format)`
    /// </summary>
    
    public string DescriptionPattern { get; set; }

    /// <summary>
    /// The name of the field which is used to assign an icon to the tag.
    /// The referenced field must exist in Fields, must have a ValidValuesRule attached and must have Icon properties on each rule.
    /// Prefer the use of the `IconField` property instead of this attribute unless serializing
    /// </summary>
    
    public string IconFieldName { get; set; }

    /// <summary>
    /// The field which is used for the presentation of an icon associated with each tag.
    /// </summary>
    [JsonIgnore]
    public Field IconField {
        get {
            return Fields.FirstOrDefault(e => e.DisplayName == IconFieldName);
        }
    }

    /// <summary>
    /// The description for the template, may be displayed to users as tooltip or additional information.
    /// </summary>
    
    public string Description { get; set; }

    /// <summary>
    /// Subtitle Text for the Template to display progress
    /// </summary>
    
    public string SubtitleText { get; set; }

    /// <summary>
    /// Addible Flag to determine if new tags can be added to this template.
    /// </summary>
    
    public bool Addible { get; set; }

    /// <summary>
    /// Flag to determine if this template is hidden on the Dashboard.
    /// (Currently for Capture)
    /// </summary>
    
    public bool HideOnDashboard { get; set; }

    /// <summary>
    /// The fields for the template that correspond to values on tags.
    /// </summary>
    public FieldCollection Fields { get; private set; }

    /// <summary>
    /// The form that is used to present the tags to users.
    /// There is only one form for all platforms, it is designed to be responsive to scale up and down as necessary.
    /// </summary>
    public Form Form {
        get { return form; }
        set {
            // repetitive pattern to assign and remove parent property from child.
            if(value == form) {
                return;
            }
            if(form != null) {
                form.ParentTemplate = null;
            }
            form = value;
            if(form != null) {
                form.ParentTemplate = this;
            }
        }
    }

    private Form form;

    /// <summary>
    /// Used as a helper for Asset.ToString() (i.e. Asset.ToString(Asset.Summary.Title)).
    /// extracts information from the asset to display in summaries etc.
    /// </summary>
    public Summary FetchSummary()
    {
        if(string.IsNullOrEmpty(SummaryReference)) {
            return null;
        }

        Guid guid;
        bool isGuid = Guid.TryParse(SummaryReference, out guid);

        if(isGuid) {
            return FetchConfiguration().Summaries.FirstOrDefault(e => e.Id == guid);
        }
        else {
            return FetchConfiguration().Summaries.FirstOrDefault(e => e.SummaryName == SummaryReference);
        }
    }

    /// <summary>
    /// The name or Guid of the summary that this tag is an instance of.
    /// </summary>
    public string SummaryReference { get; set; }

    /// <summary>
    /// The partitions that define logical groupings of data in a logical order.
    /// This is used by Zuuse Capture to display the drill down pages.
    /// It could be used for any other purposes as it's just a logical set of groups.
    /// </summary>
    public PartitionCollection Partitions { get; private set; }

    /// <summary>
    /// The table layout that is used to present a list of tags.
    /// There is only one table layout for all platforms, it is designed to be responsive to scale up and down as
    /// necessary.
    /// </summary>
    public Table Table { get; set; }

    /// <summary>
    /// Fetches the LocalResolver for the UserModel
    /// </summary>
    public IResolver FetchResolver()
    {
        return FetchConfiguration().Resolver;
    }

    public bool Embeddable { get; set; }

    /// <summary>
    /// Fetches the parent ZuuseConfiguration.Instance for the UserModel
    /// </summary>
    public ZuuseConfiguration FetchConfiguration()
    {
        return ZuuseConfiguration.Instance;
    }

    /// <summary>
    /// The Tab for the Template
    /// </summary>
    public Tab Tab { get; set; }

    /// <summary>
    /// Used to determine if the template is part the system or has been custom built.
    /// </summary>
    public bool BuiltIn { get; set; }

    /// <summary>
    /// Used to determine if the template relates to an <see cref="Entity"/>
    /// </summary>
    /// <remarks>
    /// Only a subset of the built in templates will be <see cref="Entity"/>.
    /// All custom defined templates will be a <see cref="Tag"/>
    /// </remarks>
    public bool IsEntity { get; set; }

    /// <summary>
    /// A collection of TemplateCondition which can be utilised in the
    /// <see cref="RuleSet"/> of a <see cref="Field"/>.
    /// </summary>
    public TemplateConditionCollection Conditions { get; private set; }
}
