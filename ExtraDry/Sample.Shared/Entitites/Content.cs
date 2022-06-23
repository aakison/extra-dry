namespace Sample.Shared;

public class Content {
    [Key]
    [Rules(RuleAction.Block)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    [Display(Name = "Title", ShortName = "Title")]
    public string Title { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ContentLayout? Layout { get; set; }

    /// <summary>
    /// The version info which informs the audit log.
    /// </summary>
    [JsonIgnore]
    public VersionInfo Version { get; set; } = new VersionInfo();

}
