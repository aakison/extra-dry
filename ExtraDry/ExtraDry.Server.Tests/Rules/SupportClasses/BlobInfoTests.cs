using System.Text.Json;

namespace ExtraDry.Server.Tests.Rules;

public class BlobInfoTests {

    [Fact]
    public void ValidateBlob()
    {
        var blob = ValidBlob;
        var validator = new DataValidator();

        validator.ValidateObject(blob);

        Assert.Empty(validator.Errors);
    }

    [Theory]
    [InlineData("Id", 1)]
    [InlineData("Scope", BlobScope.Private)]
    [InlineData("ShaHash", "X")]
    [InlineData("Url", "X")]
    [InlineData("Filename", "X")]
    [InlineData("MimeType", "X")]
    [InlineData("Size", 123)]
    public void RoundtripProperties(string propertyName, object propertyValue)
    {
        var blob = ValidBlob;
        var property = blob.GetType().GetProperty(propertyName);

        property?.SetValue(blob, propertyValue);
        var result = property?.GetValue(blob);

        Assert.Equal(propertyValue, result);
    }

    [Fact]
    public void RoundtripUniqueId()
    {
        var blob = ValidBlob;
        var guid = Guid.NewGuid();
            
        blob.Uuid = guid;

        Assert.Equal(guid, blob.Uuid);
    }

    [Fact]
    public void IdDoesNotLeakToJson()
    {
        var blob = ValidBlob;
        blob.Id = 12345;

        var json = JsonSerializer.Serialize(blob);

        Assert.DoesNotContain("12345", json);
    }

    [Theory]
    [InlineData("0123456789012345678901234567890123456789012345678901234567890123")]
    public void ValidShaHash(string value)
    {
        var blob = ValidBlob;
        blob.ShaHash = value;
        var validator = new DataValidator();

        var valid = validator.ValidateObject(blob);

        Assert.True(valid);
    }

    [Theory]
    [InlineData("012345678901234567890123456789012345678901234567890123456789")] // too short
    [InlineData("0123456789012345678901234567890123456789012345678901234567890123456789")] // too long
    [InlineData("012345678901234567890123456789012345678901234567890123456789012X")] // bad character
    public void InvalidShaHash(string value)
    {
        var blob = ValidBlob;
        blob.ShaHash = value;
        var validator = new DataValidator();

        var valid = validator.ValidateObject(blob);

        Assert.False(valid);
    }

    private static BlobInfo ValidBlob => new();

}
