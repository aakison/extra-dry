using System.Collections.ObjectModel;

namespace ExtraDry.Core;

public class ContentLayout
{
    public Collection<ContentSection> Sections { get; set; } = [];
}

public class ContentSection
{
    public SectionLayout Layout { get; set; } = SectionLayout.SingleColumn;

    public ContentTheme Theme { get; set; } = ContentTheme.Light;

    public Collection<ContentContainer> Containers { get; set; } = [];

    [JsonIgnore]
    public IEnumerable<ContentContainer> DisplayContainers {
        get {
            while(Containers.Count < ContainerCount) {
                Containers.Add(new ContentContainer());
            }
            return Containers.Take(ContainerCount);
        }
    }

    private int ContainerCount => Layout switch {
        SectionLayout.SingleColumn => 1,
        SectionLayout.TripleColumn => 3,
        SectionLayout.QuadrupleColumn => 4,
        _ => 2,
    };
}

public class ContentContainer
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public ContentAlignment Alignment { get; set; } = ContentAlignment.TopLeft;

    public ContentPadding Padding { get; set; } = ContentPadding.None;

    /// <summary>
    /// The HTML for the content element. This HTML should be kept very clean, most tags and styles
    /// are invalid.
    /// </summary>
    public string Html { get; set; } = string.Empty;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContentAlignment
{
    TopLeft,

    TopCenter,

    TopRight,

    MiddleLeft,

    MiddleCenter,

    MiddleRight,

    BottomLeft,

    BottomCenter,

    BottomRight,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContentTheme
{
    Light,

    Dark,

    Accent,

    Banner,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SectionLayout
{
    [Display(Name = "single")]
    SingleColumn,

    [Display(Name = "double")]
    DoubleColumn,

    [Display(Name = "triple")]
    TripleColumn,

    [Display(Name = "double left")]
    DoubleWeightedLeft,

    [Display(Name = "double right")]
    DoubleWeightedRight,

    [Display(Name = "quadruple")]
    QuadrupleColumn,
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContentPadding
{
    [Display(Name = "none")]
    None,

    [Display(Name = "regular")]
    Regular,

    [Display(Name = "large")]
    Large,
}
