namespace ExtraDry.Server.Tests.Rules;

[SoftDeleteRule(nameof(Active), ActiveType.Deleted, ActiveType.Active)]
public class User {

    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    public ActiveType Active { get; set; } = ActiveType.Pending;

    public string Name { get; set; } = string.Empty;

    public Address? Address { get; set; }

}
