namespace ExtraDry.Core;

public class ExpandoSection {

    public string Title { get; set; } = "Custom Fields";

    public List<ExpandoField> Fields { get; set; } = new();

    public int Order { get; set; }

    public ExpandoState State { get; set; } = ExpandoState.Draft;

}
