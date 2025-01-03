using System.ComponentModel.DataAnnotations.Schema;

namespace ExtraDry.Server.DataWarehouse;

[Table("__EDDataFactorySync")]
public class DataTableSync
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    /// <summary>
    /// The name of the target table.
    /// </summary>
    [Required, StringLength(100)]
    public string Table { get; set; } = null!;

    /// <summary>
    /// The schema for the table in JSON format, including any base data.
    /// </summary>
    [Required]
    public string Schema { get; set; } = null!;

    /// <summary>
    /// Represents the most recent record updated into the data warehouse. This is typically the
    /// VersionInfo.DateModified from the source entity. For enums, this is the time the entity was
    /// last modified and loaded.
    /// </summary>
    public DateTime SyncTimestamp { get; set; }
}
