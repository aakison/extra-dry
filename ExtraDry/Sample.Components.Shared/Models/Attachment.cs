using System.ComponentModel.DataAnnotations;

namespace Sample.Components;

public class Attachment
{
    public string Partition { get; set; } = "";

    [Key]
    public Guid Uuid { get; set; }

    public string Name { get; set; } = "";

    public string MimeType { get; set; } = "";

    public string? Description { get; set; }

    public byte[] Data { get; set; } = [];

    public string? Url { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string? PreviewUrl { get; set; }

    public string? IconUrl { get; set; }

    public string? Tags { get; set; }

    public string? CustomFields { get; set; }

    public string? Version { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted { get; set; }

    public string? DeletedReason { get; set; }

    public string? DeletedByIp { get; set; }

    public string? CreatedByIp { get; set; }

    public string? UpdatedByIp { get; set; }

    public string? DeletedByIpLocation { get; set; }

    public string? CreatedByIpLocation { get; set; }

    public string? UpdatedByIpLocation { get; set; }

    public string? CreatedByUserAgent { get; set; }

    public string? UpdatedByUserAgent { get; set; }

    public string? DeletedByUserAgent { get; set; }

    public string? CreatedByUserAgentOrigin { get; set; }

    public string? UpdatedByUserAgentOrigin { get; set; }

    public string? DeletedByUserAgentOrigin { get; set; }

    public string? CreatedByUserAgentDevice { get; set; }

    public string? UpdatedByUserAgentDevice { get; set; }

    public string? DeletedByUserAgentDevice { get; set; }

    public string? CreatedByUserAgentOs { get; set; }

    public string? UpdatedByUserAgentOs { get; set; }

    public string? DeletedByUserAgentOs { get; set; }
}
