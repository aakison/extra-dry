using System.Xml.Serialization;

namespace ExtraDry.Core;

/// <summary>
/// Represents a detail form for the display of Tag data.
/// This contains the fields that needs to be shown along with a suggested layout.
/// The limitations of different devices may affect the way that these layout suggestions are implement.
/// In essence, this is intended to be a responsive app form mechanism.
/// </summary>
public class Form {

    /// <summary>
    /// Creates a new form for holding the information related to a Tag.
    /// </summary>
    public Form()
    {
        Initialize();
    }

    private void Initialize()
    {
        Groups = new FormGroupCollection();
        Groups.OnInsert = group => group.AncestorForm = this;
        Groups.OnRemove = group => group.AncestorForm = null;
    }

    /// <summary>
    /// The name of the form that the form renderer will likely put as the title to the page.
    /// </summary>
    
    public string Name { get; set; }

    /// <summary>
    /// The elements that make up this form.
    /// Typically, this will be a collection of Form-Groups for organizing the Form-Controls.
    /// </summary>
    public FormGroupCollection Groups { get; private set; }

    /// <summary>
    /// The Template parent of this form.
    /// </summary>
    [XmlIgnore, JsonIgnore]
    public Template ParentTemplate { get; internal set; }

    /// <summary>
    /// A form has the interface concept of being "completed".
    /// This is an arbitrary definition and is used to manage a single state of not-done versus done for display in Zuuse Capture.
    /// The regex for tag completed is compared against the Tag.ToString() value which is a concatiniation of all the tags values.
    /// WARNING: This may be removed in the future depending on discussions around Workflow management and if Workflow may provide a better solution.
    /// NOTE: This really should be a Regex, but the Regex class is not serializable (how stupid is that, in JavaScript regex is native, but in C# it can't even be serialized).
    /// </summary>
    
    public string TagCompletedRegex { get; set; }
}
