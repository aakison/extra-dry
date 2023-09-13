namespace ExtraDry.Core;

/// <summary>
/// A FormGroup which defines the main structural layout of a form.
/// </summary>
public class FormGroup {

    /// <summary>
    /// Creates a new form group to hold a tree of groups and/or form controls.
    /// </summary>
    public FormGroup()
    {
        Initialize();
    }

    private void Initialize()
    {
        Groups = new FormGroupCollection();
        Groups.OnInsert = group => { group.ParentGroup = this; group.AncestorForm = AncestorForm; Controls = null; };
        Groups.OnRemove = group => { group.ParentGroup = null; group.AncestorForm = null; };
        Controls = new FormControlCollection();
        Controls.OnInsert = control => { Groups = null; control.ParentGroup = this; };
        Controls.OnRemove = control => control.ParentGroup = null;
    }

    /// <summary>
    /// The optional name of the group of fields.
    /// Some layouts may choose not to display this name at all, others might use it as title text.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The preferred orientation for this group.
    /// Elements in this group will be laid out using this orientation if possible.
    /// However, it is up to the actual implementation to either respect or ignore this.
    /// The most likely reason is that a group is being requested to be laid out horizontally and there isn't enough
    /// screen space, so the form lays it out vertically instead.
    /// </summary>
    public FormOrientation Orientation { get; set; }

    /// <summary>
    /// The requested alignment for this group.
    /// </summary>
    public Alignment Alignment { get; set; }

    /// <summary>
    /// Indicates any highlighting that should be used for the group.
    /// This can be used to supply subtle darker or lighter backgrounds.
    /// Additionally, a warning can be used to set off specific information such as an Asbestos warning.
    /// </summary>
    public FormHighlight Highlight { get; set; }

    /// <summary>
    /// The groups that are children of this group and are displayed inside of it, this defines a tree of groups.
    /// If a group contains Groups, then it may not contain Controls
    /// </summary>
    public FormGroupCollection Groups { get; private set; }

    /// <summary>
    /// The controls that are children of this group and are displayed inside of it.
    /// If a group contains Controls, then it may not contain Groups.
    /// </summary>
    public FormControlCollection Controls { get; private set; }

    /// <summary>
    /// The parent group for this group.  If this is a top-level group or this group has not been assigned, then this will be null.
    /// </summary>
    [JsonIgnore]
    public FormGroup ParentGroup { get; internal set; }

    /// <summary>
    /// The form ancestor for this group.
    /// </summary>
    [JsonIgnore]
    public Form AncestorForm {
        get {
            return ancestorForm;
        }
        internal set {
            ancestorForm = value;
            if(Groups != null) {
                foreach(var group in Groups) {
                    group.AncestorForm = value;
                }
            }
        }
    }

    private Form ancestorForm;

    /// <summary>
    /// The content type of the <see cref="FormGroup"/>.
    /// </summary>
    [Required]
    [EnumDataType(typeof(GroupContentType), ErrorMessage = ValidationExpression.GroupContentTypeMessage)]
    public GroupContentType ContentType { get; set; }
}
