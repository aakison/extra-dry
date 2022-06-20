using System.Text.Json;

namespace ExtraDry.Core.Models;

/// <summary>
/// A lightweight, not fully-functional replacement for String when case-sensitivity is not desired.
///
/// Within most database implementations strings are case-insensitive.  However, most memory
/// implementations treat strings as case-sensitive.  This can cause particular problems when testing
/// as in-memory test cases will behave differently than production databases.
///
/// CaselessString is designed for those limited cases where in-memory data access is required and
/// it needs to behave as a case-insensitive database would.
/// </summary>
[JsonConverter(typeof(CaselessStringJsonConverter))]
public class CaselessString {

    public static implicit operator CaselessString(string source)
    {
        return new CaselessString(source);
    }

    public CaselessString(string init)
    {
        Value = init;
    }

    public bool Equals(string rhs) => Value.Equals(rhs, StringComparison.OrdinalIgnoreCase);

    public bool Contains(string rhs) => Value.Contains(rhs, StringComparison.OrdinalIgnoreCase);

    public bool StartsWith(string rhs) => Value.StartsWith(rhs, StringComparison.OrdinalIgnoreCase);

    public string Value { get; private set; }

}

/// <summary>
/// A caseless string should serialize as a regular string.
/// </summary>
public class CaselessStringJsonConverter : JsonConverter<CaselessString> {
    public override CaselessString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            new(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, CaselessString caselessString, JsonSerializerOptions options) =>
            writer.WriteStringValue(caselessString.Value);
}
