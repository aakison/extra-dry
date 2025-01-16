using ExtraDry.Core.Internal;

namespace ExtraDry.Core;

/// <summary>
/// Represents a Binary, Large OBject (BLOB) that is typically stored in a file system or large
/// object cloud account (e.g. Azure Storage Account). This Blob is suitable for most file storage
/// solutions, but can be extended or replaced with a custom implementation if necessary.
/// </summary>
public class Blob : IBlob, IValidatableObject, ITenanted
{
    public const string UuidHeaderName = "X-Blob-Uuid";

    public const string TenantHeaderName = "X-Blob-Tenant";

    public const string SlugHeaderName = "X-Blob-Slug";

    public const string TitleHeaderName = "X-Blob-Title";

    public const string OwnerHeaderName = "X-Blob-Owner";

    public const string OwnerTypeHeaderName = "X-Blob-Owner-Type";

    /// <inheritdoc />
    [HttpHeader(TenantHeaderName)]
    public string Tenant { get; set; } = "";

    /// <inheritdoc />
    [HttpHeader(UuidHeaderName)]
    public Guid Uuid { get; set; } = Guid.CreateVersion7();

    /// <inheritdoc />
    [Required, StringLength(64)]
    [HttpHeader(SlugHeaderName)]
    public string Slug { get; set; } = "";

    /// <summary>
    /// The title of a Blob is the original Filename that was created as reported by the user.
    /// These titles are typically unsafe for web use and for security reasons the actual name of
    /// the file is not used in the URI. Instead, the title is used for display purposes only.
    /// </summary>
    [StringLength(StringLength.Line)]
    [HttpHeader(TitleHeaderName)]
    public string Title { get; set; } = "";

    [HttpHeader(OwnerHeaderName)]
    public Guid OwnerUuid { get; set; } = Guid.Empty;

    [StringLength(StringLength.Line)]
    [HttpHeader(OwnerTypeHeaderName)]
    public string OwnerType { get; set; } = "";

    /// <inheritdoc />
    [RegularExpression(@"\w+/[-+.\w]+"), StringLength(128)]
    [HttpHeader("Content-Type")]
    public string MimeType { get; set; } = "application/octet-string";

    /// <inheritdoc />
    [StringLength(32), RegularExpression("[A-F0-9]{32}")]
    [HttpHeader("Content-Md5")]
    public string MD5Hash { get; set; } = "";

    /// <summary>
    /// The length of the content of the Blob in bytes.
    /// </summary>
    [HttpHeader("Content-Length")]
    public int Length { get; set; }

    /// <summary>
    /// The actual content of the Blob.
    /// </summary>
    public byte[]? Content { get; set; }

    /// <summary>
    /// Custom validation for Blobs, ensuring MD5 is valid if provided and length matches content.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Content != null) {
            if(Length != Content.Length) {
                yield return new ValidationResult("Length does not match content", [nameof(Length)]);
            }
            if(MD5Hash != string.Empty) {
                var hash = MD5Core.GetHashString(Content);
                if(!MD5Hash.Equals(hash, StringComparison.OrdinalIgnoreCase)) {
                    yield return new ValidationResult("MD5 does not match content", [nameof(MD5Hash)]);
                }
            }
        }
    }
}
