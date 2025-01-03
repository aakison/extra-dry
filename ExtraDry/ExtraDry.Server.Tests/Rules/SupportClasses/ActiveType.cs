namespace ExtraDry.Server.Tests.Rules;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActiveType
{
    Pending,

    Inactive,

    Active,

    Deleted,
}
