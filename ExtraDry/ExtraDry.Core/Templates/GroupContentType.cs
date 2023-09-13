namespace ExtraDry.Core; 
public enum GroupContentType
{
    /// <summary>
    /// The <see cref="FormGroup"/> contains one or more <see cref="FormGroup"/>
    /// </summary>
    Groups = 0,
    /// <summary>
    /// The <see cref="FormGroup"/> contains one or more <see cref="FormControl"/>
    /// </summary>
    Fields,
    /// <summary>
    /// The <see cref="FormGroup"/> contains a conversation control.
    /// </summary>
    Conversation,
    /// <summary>
    /// The <see cref="FormGroup"/> contains possible workflows.
    /// </summary>
    Workflow,
    /// <summary>
    /// The <see cref="FormGroup"/> contains attachments.
    /// </summary>
    Attachments,
    /// <summary>
    /// The <see cref="FormGroup"/> contains tag images.
    /// </summary>
    TagImages,
    /// <summary>
    /// The <see cref="FormGroup"/> contains asset images.
    /// </summary>
    AssetImages,
}
