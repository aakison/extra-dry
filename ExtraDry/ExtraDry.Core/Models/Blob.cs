namespace ExtraDry.Core;

public interface IBlobInfo : IResourceIdentifiers
{

    public string Url { get; set; }

    [StringLength(256)]
    public string MimeType { get; set; }

}
