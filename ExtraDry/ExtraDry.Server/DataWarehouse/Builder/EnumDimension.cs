namespace ExtraDry.Server.DataWarehouse.Builder;

[DimensionTable]
public class EnumDimension {

    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(50)]
    public string? ShortName { get; set; }

    [StringLength(50)]
    public string? GroupName { get; set; }

    public int Order { get; set; }

}
