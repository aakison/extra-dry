namespace ExtraDry.Server.Tests.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ModelType {
    Phonetic,
    Greek,
    Hendrix,
    Latin,
}
