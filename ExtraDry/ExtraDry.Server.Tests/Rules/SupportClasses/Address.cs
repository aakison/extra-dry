namespace ExtraDry.Server.Tests.Rules;

[DeleteRule(DeleteAction.Recycle, nameof(Active), ActiveType.Deleted, ActiveType.Active)]
public class Address
{
    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public ActiveType Active { get; set; } = ActiveType.Pending;

    public string Line { get; set; } = string.Empty;
}
