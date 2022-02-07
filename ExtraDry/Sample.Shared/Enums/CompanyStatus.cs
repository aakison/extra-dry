#nullable enable

using System.Text.Json.Serialization;

namespace Sample.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CompanyStatus {

    Inactive,

    Active,

    Deleted,

}
