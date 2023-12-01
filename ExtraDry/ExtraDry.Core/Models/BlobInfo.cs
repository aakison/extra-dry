using System.Security.Cryptography;

namespace ExtraDry.Core;

public class BlobInfo : IBlobInfo, IResourceIdentifiers
{

    [Key]
    [Rules(RuleAction.Block)]
    [JsonIgnore]
    public int Id { get; set; }

    [Rules(RuleAction.Block)]
    public Guid Uuid { get; set; } = Guid.NewGuid();

    [Filter]
    public BlobScope Scope { get; set; }

    public int Size { get; set; }

    [StringLength(64), RegularExpression("[A-F0-9]{64}")]
    public string ShaHash { get; set; } = string.Empty;

    public void SetShaHashFromContent(byte[] bytes)
    {
        using var sha = new SHA256Managed();
        var hash = sha.ComputeHash(bytes);
        ShaHash = string.Concat(hash.Select(b => b.ToString("x2")));
    }

    /// <summary>
    /// The Url for the blob that allows direct access without using this API.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    [Display(Name = "Name", ShortName = "Name")]
    [Filter]
    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// The mime type of the blob which is delivered along with the blob when requested.
    /// Defaults to `application/octet-string` which is the most generic option.
    /// </summary>
    public string MimeType { get; set; } = "application/octet-string";

    public const string SlugHeaderName = "X-Ed-Blob-Slug";

    public const string TitleHeaderName = "X-Ed-Blob-Title";

    public const string ScopeHeaderName = "X-Ed-Blob-Scope";

    public const string UuidHeaderName = "X-Ed-Blob-Uuid";

    public const string ShaHashHeaderName = "X-Ed-Blob-Sha-Hash";

    public const string MimeTypeHeaderName = "Content-Type";

}
