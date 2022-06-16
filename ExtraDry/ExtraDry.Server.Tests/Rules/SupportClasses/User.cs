namespace ExtraDry.Server.Tests.Rules;

public class User {

    [Key]
    [JsonIgnore]
    public int Id { get; set; }

    [Rules(DeleteValue = ActiveType.Deleted)]
    public ActiveType Active { get; set; } = ActiveType.Pending;

    public string Name { get; set; } = string.Empty;

    public Address? Address { get; set; }

}
