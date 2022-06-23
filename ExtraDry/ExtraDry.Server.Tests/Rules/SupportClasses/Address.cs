namespace ExtraDry.Server.Tests.Rules;

public class Address {

    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Rules(DeleteValue = ActiveType.Deleted)]
    public ActiveType Active { get; set; } = ActiveType.Pending;

    public string Line { get; set; } = string.Empty;

}
